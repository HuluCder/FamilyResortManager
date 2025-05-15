using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FamilyResortManager.Data.DataBase;
using FamilyResortManager.Data.DataBase.Models;

namespace FamilyResortManager.Pages.Admin.Tasks
{
    [Authorize(Roles = "Administrator")]
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;

        public IndexModel(AppDbContext db)
        {
            _db = db;
        }

        public List<TaskEntry> Tasks { get; set; } = new();

        public async Task OnGetAsync()
        {
            Tasks = await _db.TaskEntries
                .Include(t => t.AssignedToUser)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }
    }
}