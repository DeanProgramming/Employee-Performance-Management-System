using EmployeePerformanceApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EmployeePerformanceApp.StoredProcedures
{
    public class GetTop5EmployeesAverageScore
    {
        private readonly EmployeePerformanceDbContext _context;

        public GetTop5EmployeesAverageScore(EmployeePerformanceDbContext context)
        {
            _context = context;
        }

        public async Task<List<TopEmployee>> GetTop5EmployeesAverageScoreAsync()
        {
            var topEmployees = new List<TopEmployee>();

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "GetTop5EmployeesAverageScore";
                command.CommandType = CommandType.StoredProcedure;

                await _context.Database.OpenConnectionAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        topEmployees.Add(new TopEmployee
                        {
                            employee_id = reader.GetInt32(0),
                            first_name = reader.GetString(1),
                            last_name = reader.GetString(2),
                            average_score = reader.GetInt32(3)
                        });
                    }
                }

                await _context.Database.CloseConnectionAsync();
            }

            return topEmployees;
        }
    }

    public class TopEmployee
    {
        public int employee_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int average_score { get; set; }
    }
}