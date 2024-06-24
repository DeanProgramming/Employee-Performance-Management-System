using EmployeePerformanceApp.Data;
using EmployeePerformanceApp.StoredProcedures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Dynamic;

namespace EmployeePerformanceApp.Pages
{
    [Authorize(Roles = "Manager, HR")]
    public class ManagerMenuModel : PageModel
    {
        private readonly EmployeePerformanceDbContext _context;
        private readonly GetTop5EmployeesAverageScore _topEmployeesRepo;
        private readonly GetEmployeeDueForReview _dueForReviewRepo;
        private readonly UserManager<IdentityUser> _userManager;

        public ManagerMenuModel(EmployeePerformanceDbContext dbContext, GetTop5EmployeesAverageScore topEmployeesRepo, GetEmployeeDueForReview dueForReviewRepo, UserManager<IdentityUser> userManager)
        {
            _context = dbContext;
            _topEmployeesRepo = topEmployeesRepo;
            _dueForReviewRepo = dueForReviewRepo;
            _userManager = userManager;
        } 

        public enum PageState
        {
            DisplayAllTeamsReviews,
            DisplayHRTeamReviews,
            DisplayTeamAReviews,
            DisplayTeamBReviews,
            DisplaySupportReviews,
            EmployeesDueForReview,
            Top5Employees
        }

        [BindProperty(SupportsGet = true)]
        public PageState CurrentState { get; set; } = PageState.DisplayAllTeamsReviews;

        public List<TopEmployee> TopEmployees { get; set; }
        public List<EmployeeRequiring> EmployeesDueForReview { get; set; }
        public List<EmployeePerformanceData> EmployeeReviews { get; set; } 
        public class EmployeePerformanceData
        {
            public string EmployeeName { get; set; }
            public List<PerformanceData> Reviews { get; set; }
        }

        public class PerformanceData
        {
            public DateTime ReviewDate { get; set; }
            public int Score { get; set; }
        }

        public bool IsHR { get; set; }

        [BindProperty]
        public int MonthCutOff { get; set; }
        public string Message { get; set; }

        public async Task OnGetAsync()
        {
            await SetHRStatusAsync();

            if (TempData.TryGetValue("CurrentState", out var state))
            {
                if (Enum.TryParse<PageState>(state.ToString(), out var currentState))
                {
                    CurrentState = currentState;
                }
            }

            if (CurrentState == PageState.DisplayHRTeamReviews)
            {
                EmployeeReviews = await GetDepartmentEmployeeReviews(1); 
            }
            else if (CurrentState == PageState.DisplayTeamAReviews)
            {
                EmployeeReviews = await GetDepartmentEmployeeReviews(2);
            }
            else if (CurrentState == PageState.DisplayTeamBReviews)
            {
                EmployeeReviews = await GetDepartmentEmployeeReviews(3);
            }
            else if (CurrentState == PageState.DisplaySupportReviews)
            {
                EmployeeReviews = await GetDepartmentEmployeeReviews(4);
            }
            else if (CurrentState == PageState.DisplayAllTeamsReviews)
            {
                EmployeeReviews = await GetAllEmployeeReviews();
            }
        }

        public async Task<IActionResult> OnPostAsync(string state)
        {
            if (Enum.TryParse<PageState>(state, out var newState))
            {
                CurrentState = newState;
            }

            TempData["CurrentState"] = CurrentState.ToString();

            return RedirectToPage();
        }

        public async Task<List<EmployeePerformanceData>> GetDepartmentEmployeeReviews(int department)
        {
            List<EmployeePerformanceData> employeesData = new List<EmployeePerformanceData>();
            employeesData = await _context.EmployeeInfos
                .Join(
                    _context.EmployeeDepartments,
                    e => e.employee_id,
                    ed => ed.employee_id,
                    (e, ed) => new { EmployeeInfo = e, EmployeeDepartment = ed }
                )
                .Where(joined => joined.EmployeeDepartment.department_id == department)
                .Select(joined => new EmployeePerformanceData
                {
                    EmployeeName = joined.EmployeeInfo.first_name + " " + joined.EmployeeInfo.last_name,
                    Reviews = _context.PerformanceReviews
                        .Where(pr => pr.employee_id == joined.EmployeeInfo.employee_id)
                        .OrderByDescending(pr => pr.review_date)
                        .Select(pr => new PerformanceData
                        {
                            ReviewDate = pr.review_date,
                            Score = pr.score
                        }).ToList()
                }).ToListAsync();

            return employeesData;
        }

        public async Task<List<EmployeePerformanceData>> GetAllEmployeeReviews()
        {
            List<EmployeePerformanceData> employeesData = new List<EmployeePerformanceData>();
            employeesData = await _context.EmployeeInfos
                .Join(
                    _context.EmployeeDepartments,
                    e => e.employee_id,
                    ed => ed.employee_id,
                    (e, ed) => new { EmployeeInfo = e, EmployeeDepartment = ed }
                ) 
                .Select(joined => new EmployeePerformanceData
                {
                    EmployeeName = joined.EmployeeInfo.first_name + " " + joined.EmployeeInfo.last_name,
                    Reviews = _context.PerformanceReviews
                        .Where(pr => pr.employee_id == joined.EmployeeInfo.employee_id)
                        .OrderByDescending(pr => pr.review_date)
                        .Select(pr => new PerformanceData
                        {
                            ReviewDate = pr.review_date,
                            Score = pr.score
                        }).ToList()
                }).ToListAsync();

            return employeesData;
        }

        public async Task OnPostGetTop5EmployeesAsync()
        {
            await SetHRStatusAsync();
            TopEmployees = await _topEmployeesRepo.GetTop5EmployeesAverageScoreAsync();
            CurrentState = PageState.Top5Employees;
            TempData["CurrentState"] = CurrentState.ToString();
        }

        public async Task OnPostGetEmployeesDueForReviewAsync()
        {
            await SetHRStatusAsync();
            EmployeesDueForReview = await _dueForReviewRepo.GetEmployeesNeedingReview(MonthCutOff);
            CurrentState = PageState.EmployeesDueForReview;
            TempData["CurrentState"] = CurrentState.ToString();
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