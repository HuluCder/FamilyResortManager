using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyResortManager.Services.DTOs;
using FamilyResortManager.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace FamilyResortManager.Pages.Bookings
{
    [Authorize(Roles = "Administrator")]
    public class CreateModel : PageModel
    {
        private readonly IBookingService _bookingService;
        private readonly IRoomService    _roomService;
        private readonly IClientService  _clientService;

        public CreateModel(
            IBookingService bookingService,
            IRoomService    roomService,
            IClientService  clientService
        )
        {
            _bookingService = bookingService;
            _roomService    = roomService;
            _clientService  = clientService;
        }

        [BindProperty]
        public BookingCreateRequestDto Input { get; set; }

        // для выпадающих списков
        public IEnumerable<SelectListItem> RoomsSelectList   { get; set; }
        public IEnumerable<SelectListItem> ClientsSelectList { get; set; }

        public async Task OnGetAsync()
        {
            await PopulateSelectListsAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await PopulateSelectListsAsync();
                return Page();
            }

            await _bookingService.CreateAsync(Input);
            return RedirectToPage("./Index");
        }

        private async Task PopulateSelectListsAsync()
        {
            // Получаем все номера и формируем список <option>
            var rooms   = await _roomService.GetAllAsync();
            RoomsSelectList = rooms
                .Select(r => new SelectListItem(
                    text: $"№{r.Number} ({r.Type}) – {r.Price:C0}",
                    value: r.Id.ToString()
                ))
                .ToList();

            // Получаем всех клиентов
            var clients = await _clientService.GetAllAsync();
            ClientsSelectList = clients
                .Select(c => new SelectListItem(
                    text: $"{c.Name} ({c.Phone})",
                    value: c.Id.ToString()
                ))
                .ToList();
        }
    }
}
