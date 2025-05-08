using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyResortManager.Services.DTOs;
using FamilyResortManager.Services.Interfaces;

namespace FamilyResortManager.Pages.Bookings
{
    public class EditModel : PageModel
    {
        private readonly IBookingService _bookingService;
        private readonly IRoomService    _roomService;
        private readonly IClientService  _clientService;

        public EditModel(
            IBookingService bookingService,
            IRoomService    roomService,
            IClientService  clientService
        )
        {
            _bookingService = bookingService;
            _roomService    = roomService;
            _clientService = clientService;
        }

        [BindProperty]
        public BookingUpdateRequestDto Input { get; set; }

        public IEnumerable<SelectListItem> RoomsSelectList   { get; set; }
        public IEnumerable<SelectListItem> ClientsSelectList { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var b = await _bookingService.GetByIdAsync(id);
            if (b == null) return NotFound();

            Input = new BookingUpdateRequestDto
            {
                Id           = b.Id,
                RoomId       = b.RoomId,
                ClientId     = b.ClientId,
                CheckInDate  = b.CheckInDate,
                CheckOutDate = b.CheckOutDate,
                Status       = b.Status
            };

            await PopulateSelectListsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await PopulateSelectListsAsync();
                return Page();
            }

            await _bookingService.UpdateAsync(Input);
            return RedirectToPage("./Index");
        }

        private async Task PopulateSelectListsAsync()
        {
            // Аналогично CreateModel
            var rooms   = await _roomService.GetAllAsync();
            RoomsSelectList = rooms.Select(r => new SelectListItem(
                text: $"№{r.Number} ({r.Type}) – {r.Price:C0}",
                value: r.Id.ToString()
            ));
            var clients = await _clientService.GetAllAsync();
            ClientsSelectList = clients.Select(c => new SelectListItem(
                text: $"{c.Name} ({c.Phone})",
                value: c.Id.ToString()
            ));
        }
    }
}
