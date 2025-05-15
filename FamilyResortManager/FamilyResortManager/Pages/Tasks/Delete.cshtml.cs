using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using FamilyResortManager.Data.DataBase;
using FamilyResortManager.Data.DataBase.Models;
using System.Security.Claims;

namespace FamilyResortManager.Pages.Tasks
{
    [Authorize(Roles = "Administrator")]
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;

        public DeleteModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TaskEntry Task { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Task = await _context.TaskEntries.FirstOrDefaultAsync(t => t.Id == id);
            if (Task == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var task = await _context.TaskEntries.FindAsync(Task.Id);
            if (task == null) return NotFound();

            _context.TaskEntries.Remove(task);

            // 🔽 Добавление лога об удалении
            var log = new TaskLogEntry
            {
                TaskEntryId = task.Id,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!,
                Timestamp = DateTime.UtcNow,
                Action = "Задача удалена"
            };
            _context.TaskLogEntries.Add(log);

            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}