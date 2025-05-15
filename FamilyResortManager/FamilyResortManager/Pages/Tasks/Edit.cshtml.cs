using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using FamilyResortManager.Data.DataBase;
using FamilyResortManager.Data.DataBase.Models;
using FamilyResortManager.Data.Identity;
using System.Security.Claims;

namespace FamilyResortManager.Pages.Tasks
{
    [Authorize(Roles = "Administrator")]
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TaskEntry Task { get; set; } = new();

        public List<ApplicationUser> AvailableUsers { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Task = await _context.TaskEntries.FindAsync(id);
            if (Task == null) return NotFound();

            AvailableUsers = await _context.Users.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                AvailableUsers = await _context.Users.ToListAsync();
                return Page();
            }

            var existing = await _context.TaskEntries.AsNoTracking().FirstOrDefaultAsync(t => t.Id == Task.Id);
            if (existing == null) return NotFound();

            _context.TaskEntries.Update(Task);

            _context.TaskLogEntries.Add(new TaskLogEntry
            {
                TaskEntryId = Task.Id,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!,
                Timestamp = DateTime.UtcNow,
                Action = $"Задача отредактирована"
            });

            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}