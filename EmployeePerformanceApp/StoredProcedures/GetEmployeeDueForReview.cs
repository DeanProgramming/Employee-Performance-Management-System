using EmployeePerformanceApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EmployeePerformanceApp.StoredProcedures
{
    public class GetEmployeeDueForReview
    {
        private readonly EmployeePerformanceDbContext _context;

        public GetEmployeeDueForReview(EmployeePerformanceDbContext context)
        {
            _context = context;
        }

        public async Task<List<EmployeeRequiring>> GetEmployeesNeedingReview(int monthCutOff)
        {
            var employeeList = new List<EmployeeRequiring>();

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "GetEmployeeDueForReview";
                command.CommandType = CommandType.StoredProcedure;

                var monthCutOffParam = command.CreateParameter();
                monthCutOffParam.ParameterName = "@MonthCutOff";
                monthCutOffParam.Value = monthCutOff;
                command.Parameters.Add(monthCutOffParam);

                await _context.Database.OpenConnectionAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        employeeList.Add(new EmployeeRequiring
                        {
                            employee_id = reader.GetInt32(0),
                            first_name = reader.GetString(1),
                            last_name = reader.GetString(2),
                            position = reader.GetString(3),
                            hire_date = reader.GetDateTime(4),
                            last_review_date = reader.GetDateTime(5), 
                        });
                    }
                }

                await _context.Database.CloseConnectionAsync();
            }

            return employeeList;
        }
    }

    public class EmployeeRequiring
    {
        public int employee_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string position { get; set; }
        public DateTime hire_date { get; set; }
        public DateTime last_review_date { get; set; }
    }
}
