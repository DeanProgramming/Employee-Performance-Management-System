using System;

namespace EmployeePerformanceApp.Models
{
    public class EmployeeInfo
    {
        public int employee_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string position { get; set; }
        public string role_position { get; set; }
        public DateTime hire_date { get; set; }
    }
}
