using EmployeePerformanceApp.StoredProcedures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Dynamic;

namespace EmployeePerformanceApp.Pages
{
    [Authorize(Roles = "Manager, HR")]
    public class ManagerMenuModel : PageModel
    {
        private readonly GetTop5EmployeesAverageScore _topEmployeesRepo;
        private readonly GetEmployeeDueForReview _dueForReviewRepo;
        private readonly UserManager<IdentityUser> _userManager;

        public ManagerMenuModel(GetTop5EmployeesAverageScore topEmployeesRepo, GetEmployeeDueForReview dueForReviewRepo, UserManager<IdentityUser> userManager)
        {
            _topEmployeesRepo = topEmployeesRepo;
            _dueForReviewRepo = dueForReviewRepo;
            _userManager = userManager;
        } 

         public List<TopEmployee> TopEmployees { get; set; }
        public List<EmployeeRequiring> EmployeesDueForReview { get; set; }

        public bool IsHR { get; set; }

        [BindProperty]
        public int MonthCutOff { get; set; }
        public string Message { get; set; }

        public async Task OnGetAsync()
        {
            await SetHRStatusAsync();
        }

        public async Task OnPostGetTop5EmployeesAsync()
        {
            await SetHRStatusAsync();
            TopEmployees = await _topEmployeesRepo.GetTop5EmployeesAverageScoreAsync();
        }

        public async Task OnPostGetEmployeesDueForReviewAsync()
        {
            await SetHRStatusAsync();
            EmployeesDueForReview = await _dueForReviewRepo.GetEmployeesNeedingReview(MonthCutOff);
        }
        public IActionResult OnPostGoToPerformanceReview()
        {
            return RedirectToPage("/PerformanceReviewListing");
        }

        public IActionResult OnPostGoToAccountChange()
        {
            return RedirectToPage("/UpdateEmployeeInformation");
        }

        public IActionResult OnPostGoToCreateAccount()
        {
            return RedirectToPage("/CreateNewEmployeeEntry");
        }
        private async Task SetHRStatusAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);

            if (user != null)
            {
                if (roles.Contains("HR"))
                {
                    IsHR = true;
                }
            }
        }
    }

}