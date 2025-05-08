using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using FamilyResortManager.Services.DTOs;
using FamilyResortManager.Services.Interfaces;

namespace FamilyResortManager.Pages.Bookings
{
    public class DeleteModel : PageModel
    {
        private readonly IBookingService _service;
        public DeleteModel(IBookingService service) => _service = service;

        [BindProperty]
        public BookingResponseDto Item { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Item = await _service.GetByIdAsync(id);
            if (Item == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _service.DeleteAsync(Item.Id);
            return RedirectToPage("./Index");
        }
    }
}