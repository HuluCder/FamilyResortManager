using Microsoft.AspNetCore.Identity;

namespace FamilyResortManager.Services.Authentication
{
    public static class IdentityDataInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            
            await SeedRolesAsync(roleManager);
            await SeedDefaultUsersAsync(userManager);
        }
        
        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            // Роли в системе
            string[] roleNames = { "Administrator", "Employee" };
            
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
        
        private static async Task SeedDefaultUsersAsync(UserManager<IdentityUser> userManager)
        {
            // Создаем администратора по умолчанию
            var adminEmail = "admin@familyresort.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                
                // Пароль администратора - измените на более безопасный в боевой среде
                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Administrator");
                }
            }
        }
    }
}