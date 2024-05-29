using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmployeePerformanceApp.Models
{
    public class PerformanceReviews
    {
        [Key]
        public int review_id { get; set; }

        [ForeignKey("employee_id")]
        public int employee_id { get; set; } 

        public DateTime review_date { get; set; }
        public int score { get; set; }
        public string comments { get; set; } 
    }
}
