using EmployeePerformanceApp.StoredProcedures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;

namespace EmployeePerformanceApp.Pages
{
    public class CreateNewEmployeeEntryModel : PageModel
    {
        private readonly AddNewEmployee _addNewEmployeeRepo;

        public CreateNewEmployeeEntryModel(AddNewEmployee addNewEmployeeRepo)
        {
            _addNewEmployeeRepo = addNewEmployeeRepo;
        }

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
        public string LoginName { get; set; }
        [BindProperty]
        public string RolePosition { get; set; }
        public int NewEmployeeID { get; set; }

        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                string missingItems = "";

                missingItems = missingItems + (FirstName.IsNullOrEmpty() ? " Missing = Valid First Name ||" : "");
                missingItems = missingItems + (LastName.IsNullOrEmpty() ? " Missing = Valid Last Name ||" : "");
                missingItems = missingItems + (Position.IsNullOrEmpty() ? " Missing = Valid Position ||" : "");
                missingItems = missingItems + (LoginName.IsNullOrEmpty() ? " Missing = Valid Login Name" : ""); 

                ErrorMessage = $"An error occurred: {missingItems}";
                return Page();
            }

            try
            {
                NewEmployeeID = await _addNewEmployeeRepo.AddNewEmployeeAsync(FirstName, LastName, Position, HireDate, DepartmentID, LoginName, RolePosition);

                if (NewEmployeeID > 0)
                {
                    SuccessMessage = $"Created a new user: {FirstName} {LastName}";

                    FirstName = "";
                    LastName = "";
                    Position = "";
                    HireDate = DateTime.Now;
                    DepartmentID = 1;
                    LoginName = "";
                    RolePosition = "";
                }
                else
                {
                    ErrorMessage = "Failed to create a new user.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
            }

            return Page();
        }
    }
}
