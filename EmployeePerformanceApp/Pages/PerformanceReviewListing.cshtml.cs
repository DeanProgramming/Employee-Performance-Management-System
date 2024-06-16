using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EmployeePerformanceApp.Data;
using EmployeePerformanceApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace EmployeePerformanceApp.Pages
{
    public class PerformanceReviewListingModel : PageModel
    {
        private readonly EmployeePerformanceDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PerformanceReviewListingModel(EmployeePerformanceDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<PerformanceReviews> PerformanceReviews { get; set; }

        [BindProperty]
        public int EmployeeId { get; set; }
        public bool Admin { get; set; }
        public string UserName { get; set; }

        [BindProperty]
        public string SearchTerm { get; set; }

        public List<EmployeeInfo> Employees { get; set; } = new List<EmployeeInfo>();

        public bool EmployeeFound { get; set; } = false;

        public string SuccessMessage { get; set; }

        public string ErrorMessage { get; set; }

        private async Task SetAdminStatusAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);

            if (user != null)
            {
                if (roles.Contains("Manager"))
                {
                    Admin = true;
                }
            }
        }

        public async Task OnGetAsync()
        {
            await SetAdminStatusAsync();

            if (Admin == false)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                UserName = user.UserName;
                EmployeeId = int.Parse(user.Id);

                // Load reviews for the logged-in user
                PerformanceReviews = await _context.PerformanceReviews
                    .Where(pr => pr.employee_id == EmployeeId)
                    .OrderByDescending(pr => pr.review_date)
                    .ToListAsync();
            }
        }
         

        public async Task<IActionResult> OnPostSearchAsync()
        {
            await SetAdminStatusAsync();

            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                ErrorMessage = "Search term cannot be empty.";
                return Page();
            }

            Employees = await _context.EmployeeInfos
                .Where(e => e.first_name.Contains(SearchTerm) || e.last_name.Contains(SearchTerm) || e.position.Contains(SearchTerm))
                .ToListAsync();

            if (Employees.Count == 0)
            {
                ErrorMessage = "Couldnt find anyone under the name. " + SearchTerm; 
            }

            return Page();
        }

        public async Task<IActionResult> OnPostSelectAsync(int employeeId)
        {
            await SetAdminStatusAsync();

            var employee = await _context.EmployeeInfos.FindAsync(employeeId);

            if (employee != null)
            {
                UserName = employee.first_name + " " + employee.last_name;
                EmployeeId = employee.employee_id;
                EmployeeFound = true;

                PerformanceReviews = await _context.PerformanceReviews
                    .Where(pr => pr.employee_id == EmployeeId)
                    .OrderByDescending(pr => pr.review_date)
                    .ToListAsync();

                SuccessMessage = "Reviews loaded successfully.";
            }
            else
            {
                ErrorMessage = "Employee not found.";
            }

            return Page();
        }
    }
}
