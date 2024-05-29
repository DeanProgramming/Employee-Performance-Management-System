using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EmployeePerformanceApp.Data;
using EmployeePerformanceApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EmployeePerformanceApp.Pages
{
    public class PerformanceReviewListingModel : PageModel
    {
        private readonly EmployeePerformanceDbContext _context;

        public PerformanceReviewListingModel(EmployeePerformanceDbContext context)
        {
            _context = context;
        }

        public IList<PerformanceReviews> PerformanceReviews { get; set; }

        [BindProperty]
        public int EmployeeId { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            PerformanceReviews = await _context.PerformanceReviews
                .Where(pr => pr.employee_id == EmployeeId)
                .OrderByDescending(pr => pr.review_date)
                .ToListAsync();

            return Page();
        } 
    }
}
