using EmployeePerformanceApp.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading.Tasks;

namespace EmployeePerformanceApp.StoredProcedures
{
    public class UpdateEmployeeDetails
    {
        private readonly EmployeePerformanceDbContext _context;

        public UpdateEmployeeDetails(EmployeePerformanceDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateEmployeeDetailsAsync(int employeeId, string firstName, string lastName, string position, DateTime hireDate, int departmentID)
        {
            bool isUpdated = false;

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "UpdateEmployeeDetails";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@EmployeeID", SqlDbType.Int) { Value = employeeId });
                command.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.VarChar, 255) { Value = firstName });
                command.Parameters.Add(new SqlParameter("@LastName", SqlDbType.VarChar, 255) { Value = lastName });
                command.Parameters.Add(new SqlParameter("@Position", SqlDbType.VarChar, 255) { Value = position });
                command.Parameters.Add(new SqlParameter("@HireDate", SqlDbType.Date) { Value = hireDate });
                command.Parameters.Add(new SqlParameter("@DepartmentID", SqlDbType.Int) { Value = departmentID });

                await _context.Database.OpenConnectionAsync();

                try
                {
                    var result = await command.ExecuteNonQueryAsync();
                    isUpdated = true;
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    await _context.Database.CloseConnectionAsync();
                }
            }

            return isUpdated;
        }
    }
}
