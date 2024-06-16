using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeePerformanceApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<RedirectToPageResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Manager") || roles.Contains("HR"))
            {
                return RedirectToPage("/ManagerMenu");
            }
            else
            {
                return RedirectToPage("/PerformanceReviewListing");
            }
        }
    }
}