using EmployeePerformanceApp.StoredProcedures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace EmployeePerformanceApp.Pages
{
    [Authorize(Roles = "Manager")]
    public class ManagerMenuModel : PageModel
    {
        private readonly GetTop5EmployeesAverageScore _topEmployeesRepo;
        private readonly GetEmployeeDueForReview _dueForReviewRepo;

        public ManagerMenuModel(GetTop5EmployeesAverageScore topEmployeesRepo, GetEmployeeDueForReview dueForReviewRepo)
        {
            _topEmployeesRepo = topEmployeesRepo;
            _dueForReviewRepo = dueForReviewRepo;
        }

        public List<TopEmployee> TopEmployees { get; set; }
        public List<EmployeeRequiring> EmployeesDueForReview { get; set; }

        [BindProperty]
        public int MonthCutOff { get; set; }
        public string Message { get; set; }

        public async Task OnPostGetTop5EmployeesAsync()
        {
            TopEmployees = await _topEmployeesRepo.GetTop5EmployeesAverageScoreAsync();
        }

        public async Task OnPostGetEmployeesDueForReviewAsync()
        {
            EmployeesDueForReview = await _dueForReviewRepo.GetEmployeesNeedingReview(MonthCutOff);
        }
    }

}