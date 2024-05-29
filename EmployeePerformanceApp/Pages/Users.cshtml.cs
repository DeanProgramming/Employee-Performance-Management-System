using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EmployeePerformanceApp.Data;
using EmployeePerformanceApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeePerformanceApp.Pages
{
    public class UsersModel : PageModel
    {
        private readonly EmployeePerformanceDbContext _context;

        public UsersModel(EmployeePerformanceDbContext context)
        {
            _context = context;
        }

        public IList<EmployeeInfo> EmployeeInfoList { get; set; }

        public async Task OnGetAsync()
        {
            EmployeeInfoList = await _context.EmployeeInfos.ToListAsync();
        }
    }
}