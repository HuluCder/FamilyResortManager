using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FamilyResortManager.Data.DTOs;
using FamilyResortManager.Services.Interfaces;

namespace FamilyResortManager.Pages.Admin
{
    [Authorize(Roles = "Administrator")]
    public class EmployeeManagementModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        private readonly IUserManagementService _userManagementService;
        
        public EmployeeManagementModel(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }
        
        [BindProperty]
        public CreateEmployeeDto NewEmployee { get; set; } = new();
        
        public List<EmployeeDto> Employees { get; set; } = new();
        
        public async Task OnGetAsync()
        {
            Employees = await _userManagementService.GetAllEmployeesAsync();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Employees = await _userManagementService.GetAllEmployeesAsync();
                return Page();
            }
            
            var result = await _userManagementService.CreateEmployeeAsync(NewEmployee);
            
            if (result.Succeeded)
            {
                return RedirectToPage();
            }
            
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            
            Employees = await _userManagementService.GetAllEmployeesAsync();
            return Page();
        }
        
        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            await _userManagementService.DeleteEmployeeAsync(id);
            return RedirectToPage();
        }
    }
}