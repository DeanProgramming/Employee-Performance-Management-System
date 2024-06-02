using EmployeePerformanceApp.Data;
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

                var employeeIdParam = command.CreateParameter();
                employeeIdParam.ParameterName = "@EmployeeID";
                employeeIdParam.Value = employeeId;
                command.Parameters.Add(employeeIdParam);

                var firstNameParam = command.CreateParameter();
                firstNameParam.ParameterName = "@FirstName";
                firstNameParam.Value = firstName;
                command.Parameters.Add(firstNameParam);

                var lastNameParam = command.CreateParameter();
                lastNameParam.ParameterName = "@LastName";
                lastNameParam.Value = lastName;
                command.Parameters.Add(lastNameParam);

                var positionParam = command.CreateParameter();
                positionParam.ParameterName = "@Position";
                positionParam.Value = position;
                command.Parameters.Add(positionParam);

                var hireDateParam = command.CreateParameter();
                hireDateParam.ParameterName = "@HireDate";
                hireDateParam.Value = hireDate;
                command.Parameters.Add(hireDateParam);

                var departmentIDParam = command.CreateParameter();
                departmentIDParam.ParameterName = "@DepartmentID";
                departmentIDParam.Value = departmentID;
                command.Parameters.Add(departmentIDParam);

                await _context.Database.OpenConnectionAsync();

                try
                {
                    var result = await command.ExecuteNonQueryAsync();
                    isUpdated = result > 0;
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
