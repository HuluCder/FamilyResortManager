// Pages/Rooms/Details.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FamilyResortManager.Services.Interfaces;
using FamilyResortManager.Services.DTOs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FamilyResortManager.Pages.Rooms
{
    [Authorize(Roles = "Administrator")]
    public class DetailsModel : PageModel
    {
        private readonly IRoomService _service;
        public DetailsModel(IRoomService service) => _service = service;

        [BindProperty]
        public RoomResponseDto Item { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Item = await _service.GetByIdAsync(id);
            if (Item == null) return NotFound();
            return Page();
        }
    }
}