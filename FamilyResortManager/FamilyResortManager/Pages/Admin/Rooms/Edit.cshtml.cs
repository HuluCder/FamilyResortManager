using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FamilyResortManager.Services.Interfaces;
using FamilyResortManager.Services.DTOs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FamilyResortManager.Pages.Rooms
{
    [Authorize(Roles = "Administrator")]
    public class EditModel : PageModel
    {
        private readonly IRoomService _service;

        public EditModel(IRoomService service)
        {
            _service = service;
        }

        [BindProperty]
        public RoomUpdateRequestDto Input { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var room = await _service.GetByIdAsync(id);
            if (room == null)
                return NotFound();

            Input = new RoomUpdateRequestDto
            {
                Id     = room.Id,
                Number = room.Number,
                Type   = room.Type,
                Price  = room.Price,
                Status = room.Status
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _service.UpdateAsync(Input);
            return RedirectToPage("./Index");
        }
    }
}