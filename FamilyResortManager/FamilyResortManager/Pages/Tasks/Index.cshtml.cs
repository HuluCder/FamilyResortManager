using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using FamilyResortManager.Data.DataBase;
using FamilyResortManager.Data.DataBase.Models;
using System.Security.Claims;

namespace FamilyResortManager.Pages.Tasks
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public List<TaskEntry> Tasks { get; set; } = new();

        public string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        
        [BindProperty(SupportsGet = true)] public string? StatusFilter { get; set; }
        [BindProperty(SupportsGet = true)] public string? CategoryFilter { get; set; }
        [BindProperty(SupportsGet = true)] public string? TitleSearch { get; set; }
        [BindProperty(SupportsGet = true)] public string? SortBy { get; set; }
        [BindProperty(SupportsGet = true)] public string? SortOrder { get; set; } // "asc" или "desc"
        public List<string> AvailableStatuses { get; set; } = new();
        public List<string> AvailableCategories { get; set; } = new();

        public async Task OnGetAsync()
        {
            var query = _context.TaskEntries
                .Include(t => t.AssignedToUser)
                .AsQueryable();
            
            if (!User.IsInRole("Administrator"))
            {
                query = query.Where(t => t.AssignedToUserId == CurrentUserId);
            }

            if (!string.IsNullOrEmpty(StatusFilter))
            {
                query = query.Where(t => t.Status == StatusFilter);
            }
            if (!string.IsNullOrEmpty(CategoryFilter))
            {
                query = query.Where(t => t.Category == CategoryFilter);
            }
            if (!string.IsNullOrEmpty(TitleSearch))
            {
                query = query.Where(t => t.Title.Contains(TitleSearch));
            }
            
            query = (SortBy, SortOrder) switch
            {
                ("CreatedAt", "asc") => query.OrderBy(t => t.CreatedAt),
                ("CreatedAt", "desc") => query.OrderByDescending(t => t.CreatedAt),
                ("Priority", "asc") => query.OrderBy(t => t.Priority),
                ("Priority", "desc") => query.OrderByDescending(t => t.Priority),
                _ => query.OrderByDescending(t => t.CreatedAt)
            };
            
            Tasks = await query.ToListAsync();

            // Опции фильтров
            AvailableStatuses = await _context.TaskEntries
                .Select(t => t.Status).Distinct().ToListAsync();
            AvailableCategories = await _context.TaskEntries
                .Select(t => t.Category).Where(c => c != null).Distinct().ToListAsync();
        }

        public async Task<IActionResult> OnPostUpdateCommentAsync(int taskId, string comment)
        {
            var task = await _context.TaskEntries.FindAsync(taskId);
            if (task == null || task.AssignedToUserId != CurrentUserId) return Unauthorized();

            task.Comment = comment;
            await _context.SaveChangesAsync();

            await AddLogAsync(taskId, $"Комментарий обновлён: {comment}");
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(int taskId, string status)
        {
            var task = await _context.TaskEntries.FindAsync(taskId);
            if (task == null || task.AssignedToUserId != CurrentUserId) return Unauthorized();

            task.Status = status;
            await _context.SaveChangesAsync();

            await AddLogAsync(taskId, $"Статус изменён на: {status}");
            return RedirectToPage();
        }
        
        private async Task AddLogAsync(int taskId, string action)
        {
            var log = new TaskLogEntry
            {
                TaskEntryId = taskId,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!,
                Timestamp = DateTime.UtcNow,
                Action = action
            };
            _context.TaskLogEntries.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
