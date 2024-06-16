using EmployeePerformanceApp.Data;
using EmployeePerformanceApp.StoredProcedures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EmployeePerformanceApp.Pages
{
    [Authorize(Roles = "HR")]
    public class UpdateEmployeeInformationModel : PageModel
    {
        private readonly EmployeePerformanceDbContext _context;
        private readonly UpdateEmployeeDetails _updateEmployeeDetails;

        public UpdateEmployeeInformationModel(EmployeePerformanceDbContext context, UpdateEmployeeDetails updateEmployeeDetails)
        {
            _context = context;
            _updateEmployeeDetails = updateEmployeeDetails;
        }

        [BindProperty]
        public int EmployeeID { get; set; }

        [BindProperty]
        public string FirstName { get; set; }

        [BindProperty]
        public string LastName { get; set; }

        [BindProperty]
        public string Position { get; set; }

        [BindProperty]
        public DateTime HireDate { get; set; }

        [BindProperty]
        public int DepartmentID { get; set; }

        [BindProperty]
        public string RolePosition { get; set; }

        [BindProperty]
        public string SearchTerm { get; set; }

        public List<EmployeePerformanceApp.Models.EmployeeInfo> Employees { get; set; }

        public bool EmployeeFound { get; set; } = false;

        public string SuccessMessage { get; set; }

        public string ErrorMessage { get; set; }
         

        public async Task<IActionResult> OnPostSearchAsync()
        {
            Employees = await _context.EmployeeInfos
                .Where(e => e.first_name.Contains(SearchTerm) || e.last_name.Contains(SearchTerm) || e.position.Contains(SearchTerm))
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostSelectAsync(int employeeId)
        {
            var employee = await _context.EmployeeInfos.FindAsync(employeeId);

            if (employee != null)
            {
                EmployeeID = employee.employee_id;
                FirstName = employee.first_name;
                LastName = employee.last_name;
                Position = employee.position;
                HireDate = employee.hire_date;
                DepartmentID = _context.EmployeeDepartments
                                       .FirstOrDefault(d => d.employee_id == employee.employee_id)?.department_id ?? 0;
                RolePosition = employee.role_position;
                EmployeeFound = true;
            }
            else
            {
                ErrorMessage = "Employee not found.";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!ModelState.IsValid)
            {
                EmployeeFound = true;
                return Page();
            }

            try
            {
                bool isUpdated = await _updateEmployeeDetails.UpdateEmployeeDetailsAsync(EmployeeID, FirstName, LastName, Position, HireDate, DepartmentID);
                if (isUpdated)
                {
                    SuccessMessage = $"Successfully updated employee {FirstName} {LastName}.";
                }
                else
                {
                    ErrorMessage = "Failed to update employee details.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
            }

            EmployeeFound = true;
            return Page();
        }

        public IActionResult OnPostGoBackToMenu()
        {
            return RedirectToPage("/ManagerMenu");
        }
    }
}