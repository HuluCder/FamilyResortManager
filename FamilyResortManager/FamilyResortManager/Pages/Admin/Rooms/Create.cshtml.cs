using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FamilyResortManager.Services.Interfaces;
using FamilyResortManager.Services.DTOs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FamilyResortManager.Pages.Rooms
{
    [Authorize(Roles = "Administrator")]
    public class CreateModel : PageModel
    {
        private readonly IRoomService _service;

        public CreateModel(IRoomService service)
        {
            _service = service;
        }

        [BindProperty]
        public RoomCreateRequestDto Input { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _service.CreateAsync(Input);
            return RedirectToPage("./Index");
        }
    }
}