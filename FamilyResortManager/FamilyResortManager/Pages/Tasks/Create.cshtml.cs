using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using FamilyResortManager.Data.DataBase;
using FamilyResortManager.Data.DataBase.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using FamilyResortManager.Data.Identity;
using Microsoft.EntityFrameworkCore;

namespace FamilyResortManager.Pages.Tasks
{
    [Authorize(Roles = "Administrator")]
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly TelegramNotifier _telegramNotifier;
        
        public CreateModel(AppDbContext context, TelegramNotifier telegramNotifier)
        {
            _context = context;
            _telegramNotifier = telegramNotifier;
        }

        [BindProperty]
        public TaskEntry Task { get; set; } = new();
        public List<SelectListItem> Users { get; set; } = new();
        
        public async Task OnGetAsync()
        {
            Users = await _context.Users
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.FullName ?? u.UserName
                })
                .ToListAsync();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.TaskEntries.Add(Task);
            await _context.SaveChangesAsync();

            // 🔽 Добавление лога после создания
            var log = new TaskLogEntry
            {
                TaskEntryId = Task.Id,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!,
                Timestamp = DateTime.UtcNow,
                Action = "Задача создана"
            };

            _context.TaskLogEntries.Add(log);
            await _context.SaveChangesAsync();
            
            if (Task.AssignedToUserId != null)
            {
                var user = await _context.Users.FindAsync(Task.AssignedToUserId);
                if (user?.TelegramChatId != null)
                {
                    var message = $"📌 Вам назначена новая задача: <b>{Task.Title}</b>\nСтатус: {Task.Status}\nПриоритет: {Task.Priority}";
                    await _telegramNotifier.SendMessageAsync(user.TelegramChatId, message);
                }
            }
            
            return RedirectToPage("Index");
            
        }
    }
}