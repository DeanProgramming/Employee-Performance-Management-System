using EmployeePerformanceApp.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading.Tasks;

namespace EmployeePerformanceApp.StoredProcedures
{
    public class AddNewEmployee
    {
        private readonly EmployeePerformanceDbContext _context;

        public AddNewEmployee(EmployeePerformanceDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddNewEmployeeAsync(string firstName, string lastName, string position, DateTime hireDate, int departmentID, string loginName, string rolePosition)
        {
            int newEmployeeId = 0;

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "AddNewEmployee";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.VarChar) { Value = firstName });
                command.Parameters.Add(new SqlParameter("@LastName", SqlDbType.VarChar) { Value = lastName });
                command.Parameters.Add(new SqlParameter("@Position", SqlDbType.VarChar) { Value = position });
                command.Parameters.Add(new SqlParameter("@HireDate", SqlDbType.Date) { Value = hireDate });
                command.Parameters.Add(new SqlParameter("@DepartmentID", SqlDbType.Int) { Value = departmentID });
                command.Parameters.Add(new SqlParameter("@LoginName", SqlDbType.VarChar) { Value = loginName });
                command.Parameters.Add(new SqlParameter("@RoleName", SqlDbType.VarChar) { Value = rolePosition });
                command.Parameters.Add(new SqlParameter("@RolePosition", SqlDbType.VarChar) { Value = rolePosition });

                await _context.Database.OpenConnectionAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        newEmployeeId = reader.GetInt32(0);
                    }
                }

                await _context.Database.CloseConnectionAsync();
            }

            return newEmployeeId;
        }
    }
}
