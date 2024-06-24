using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EmployeePerformanceApp.Data;
using EmployeePerformanceApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using EmployeePerformanceApp.StoredProcedures;
using static EmployeePerformanceApp.Pages.ManagerMenuModel;

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
        public string FormattedUserName {
            get { return FormatUserName(UserName); }
        }

        [BindProperty]
        public string SearchTerm { get; set; }

        public List<EmployeeInfo> Employees { get; set; } = new List<EmployeeInfo>();

        public bool EmployeeFound { get; set; } = false;

        public string SuccessMessage { get; set; }

        public string ErrorMessage { get; set; }

        public bool CreatingNewReport { get; set; }
        [BindProperty]
        public DateTime PerformanceReviewDate { get; set; }
        [BindProperty]
        public int PerformanceRating { get; set; }
        [BindProperty]
        public string PerformanceComments { get; set; }
        [BindProperty]
        public bool UpdatePerformanceReview { get; set; }
        [BindProperty]
        public int ReviewID { get; set; }

        private async Task SetAdminStatusAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);

            if (user != null)
            {
                if (roles.Contains("Manager") || roles.Contains("HR"))
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

        public async Task<IActionResult> OnPostOpenNewReportAsync(int employeeId, string userName)
        {
            await SetAdminStatusAsync();

            if (!Admin)
            {
                return Page();
            }

            UserName = userName;
            EmployeeFound = true;
            EmployeeId = employeeId;

            CreatingNewReport = true;
            PerformanceReviewDate = DateTime.Today;
            PerformanceRating = 0;
            PerformanceComments = "";

            return Page();
        }

        public async Task<IActionResult> OnPostCancelNewReportAsync(int employeeId, string userName)
        {
            await SetAdminStatusAsync();

            if (!Admin)
            {
                return Page();
            }

            UserName = userName;
            EmployeeFound = true;
            EmployeeId = employeeId;

            CreatingNewReport = false;
            UpdatePerformanceReview = false; 

            return Page();
        }

        public async Task<IActionResult> OnPostCompleteAsync(int employeeId, string userName, int reviewID, int updatingPerforance)
        {
            await SetAdminStatusAsync();
             
            if (String.IsNullOrEmpty(PerformanceComments))
            {
                ErrorMessage = "Requires performance comments";
                return Page();
            }

            UserName = userName;
            EmployeeFound = true;
            EmployeeId = employeeId; 
            UpdatePerformanceReview = updatingPerforance == 1;

            if (UpdatePerformanceReview)
            {
                SqlParameter[] parameters = new SqlParameter[4];

                parameters[0] = new SqlParameter("@ReviewID", reviewID);
                parameters[1] = new SqlParameter("@ReviewDate", PerformanceReviewDate);
                parameters[2] = new SqlParameter("@Score", PerformanceRating);
                parameters[3] = new SqlParameter("@Comments", PerformanceComments);

                await _context.ExecuteStoredProcedureAsync("UpdatePerformanceReview", parameters);

                PerformanceReviews = await _context.PerformanceReviews
                    .Where(pr => pr.employee_id == EmployeeId)
                    .OrderByDescending(pr => pr.review_date)
                    .ToListAsync();

                SuccessMessage = "Updated Review.";
            }
            else 
            { 
                SqlParameter[] parameters = new SqlParameter[4];

                parameters[0] = new SqlParameter("@EmployeeID", EmployeeId);
                parameters[1] = new SqlParameter("@ReviewDate", PerformanceReviewDate);
                parameters[2] = new SqlParameter("@Score", PerformanceRating);
                parameters[3] = new SqlParameter("@Comments", PerformanceComments);

                await _context.ExecuteStoredProcedureAsync("InsertPerformanceReview", parameters);

                PerformanceReviews = await _context.PerformanceReviews
                    .Where(pr => pr.employee_id == EmployeeId)
                    .OrderByDescending(pr => pr.review_date)
                    .ToListAsync();

                SuccessMessage = "Added Review.";
            }



            PerformanceReviewDate = DateTime.Today;
            PerformanceRating = 0;
            PerformanceComments = "";
            CreatingNewReport = false;
            UpdatePerformanceReview = false; 

            return Page();
        }

        public async Task<IActionResult> OnPostSelectPerformanceReportAsync(int employeeId, int performanceReviewID, string userName)
        {
            await SetAdminStatusAsync();

            if (!Admin)
            {
                return Page();
            }

            UserName = userName;
            EmployeeFound = true;
            EmployeeId = employeeId;


            PerformanceReviews = await _context.PerformanceReviews
                .Where(pr => pr.review_id == performanceReviewID)
                .ToListAsync();

            PerformanceReviewDate = PerformanceReviews[0].review_date;
            PerformanceRating = PerformanceReviews[0].score;
            PerformanceComments = PerformanceReviews[0].comments;

            ReviewID = performanceReviewID;
            CreatingNewReport = false;
            UpdatePerformanceReview = true; 

            return Page();
        }

        public IActionResult OnPostGoBackToMenu()
        {
            return RedirectToPage("/ManagerMenu");
        }

        private string FormatUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return string.Empty;
            }

            var parts = userName.Split('.');
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Length > 0)
                {
                    parts[i] = char.ToUpper(parts[i][0]) + parts[i].Substring(1);
                }
            }
            return string.Join(" ", parts);
        }
    }
}
