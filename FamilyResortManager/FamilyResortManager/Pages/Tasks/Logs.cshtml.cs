using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using FamilyResortManager.Data.DataBase;
using FamilyResortManager.Data.DataBase.Models;

namespace FamilyResortManager.Pages.Tasks
{
    [Authorize]
    public class LogsModel : PageModel
    {
        private readonly AppDbContext _context;

        public LogsModel(AppDbContext context)
        {
            _context = context;
        }

        public TaskEntry? Task { get; set; }

        public List<TaskLogEntry> Logs { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int taskId)
        {
            Task = await _context.TaskEntries
                .Include(t => t.AssignedToUser)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (Task == null)
                return NotFound();

            if (!User.IsInRole("Administrator") && Task.AssignedToUserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            Logs = await _context.TaskLogEntries
                .Where(l => l.TaskEntryId == taskId)
                .Include(l => l.User)
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();

            return Page();
        }
    }
}