using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FamilyResortManager.Data.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FamilyResortManager.Pages.Account
{
    [Authorize]
    public class TelegramModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public TelegramModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public string? ChatId { get; set; }

        public string? CurrentChatId { get; set; }

        [TempData]
        public string? SuccessMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            CurrentChatId = user.TelegramChatId;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            user.TelegramChatId = ChatId;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                SuccessMessage = "✅ Chat ID успешно сохранён.";
                return RedirectToPage(); // обновим страницу
            }

            ModelState.AddModelError(string.Empty, "Не удалось сохранить Chat ID.");
            return Page();
        }
    }
}