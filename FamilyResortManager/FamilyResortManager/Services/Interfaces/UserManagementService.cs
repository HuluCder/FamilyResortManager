using Microsoft.AspNetCore.Identity;
using FamilyResortManager.Data.DTOs;
using FamilyResortManager.Services.Interfaces;

namespace FamilyResortManager.Services.Interfaces
{
    public interface IUserManagementService
    {
        Task<IdentityResult> CreateEmployeeAsync(CreateEmployeeDto model);
        Task<List<EmployeeDto>> GetAllEmployeesAsync();
        Task<IdentityResult> UpdateEmployeeAsync(string id, UpdateEmployeeDto model);
        Task<IdentityResult> DeleteEmployeeAsync(string id);
    }
}

namespace FamilyResortManager.Services.Implementation
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        
        public UserManagementService(
            UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        public async Task<IdentityResult> CreateEmployeeAsync(CreateEmployeeDto model)
        {
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };
            
            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Employee");
            }
            
            return result;
        }
        
        public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = new List<EmployeeDto>();
            var usersInRole = await _userManager.GetUsersInRoleAsync("Employee");
            
            foreach (var user in usersInRole)
            {
                employees.Add(new EmployeeDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName
                });
            }
            
            return employees;
        }
        
        public async Task<IdentityResult> UpdateEmployeeAsync(string id, UpdateEmployeeDto model)
        {
            var user = await _userManager.FindByIdAsync(id);
            
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Пользователь не найден" });
            }
            
            user.Email = model.Email;
            user.UserName = model.Email;
            
            var result = await _userManager.UpdateAsync(user);
            
            if (result.Succeeded && !string.IsNullOrEmpty(model.NewPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
            }
            
            return result;
        }
        
        public async Task<IdentityResult> DeleteEmployeeAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Пользователь не найден" });
            }
            
            return await _userManager.DeleteAsync(user);
        }
    }
}