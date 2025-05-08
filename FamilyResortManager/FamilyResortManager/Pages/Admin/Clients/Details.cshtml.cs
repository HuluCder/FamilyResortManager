using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using FamilyResortManager.Data.DataBase;
using FamilyResortManager.Data.DataBase.Models;
using FamilyResortManager.Services.DTOs;
using FamilyResortManager.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace FamilyResortManager.Pages.Clients
{
    [Authorize(Roles = "Administrator")]
    public class ClientDetailsModel : PageModel
    {
        private readonly AppDbContext _context;

        public ClientDetailsModel(AppDbContext context) => _context = context;

        public Data.DataBase.Models.Client Client { get; set; }
        public List<Booking> Bookings { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Client = await _context.Clients.FindAsync(id);
            if (Client == null) return NotFound();

            Bookings = await _context.Bookings
                .Where(b => b.ClientId == id)
                .Include(b => b.Room)
                .OrderByDescending(b => b.CheckInDate)
                .ToListAsync();

            return Page();
        }
    }
}