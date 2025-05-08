using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using FamilyResortManager.Services.DTOs;
using FamilyResortManager.Services.Interfaces;

namespace FamilyResortManager.Pages.Bookings
{
    public class DetailsModel : PageModel
    {
        private readonly IBookingService _bookingService;
        private readonly IRoomService _roomService;
        private readonly IClientService _clientService;
    
        public DetailsModel(
            IBookingService bookingService,
            IRoomService roomService,
            IClientService clientService)
        {
            _bookingService = bookingService;
            _roomService = roomService;
            _clientService = clientService;
        }
    
        [BindProperty]
        public BookingResponseDto Item { get; set; }
    
        public string RoomName { get; set; }
        public string ClientName { get; set; }
    
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Item = await _bookingService.GetByIdAsync(id);
            if (Item == null) return NotFound();
        
            // Получаем имя комнаты
            var room = await _roomService.GetByIdAsync(Item.RoomId);
            RoomName = room != null ? $"№{room.Number} ({room.Type}) – {room.Price:C0}" : "Не найдено";
        
            // Получаем имя клиента
            var client = await _clientService.GetByIdAsync(Item.ClientId);
            ClientName = client != null ? $"{client.Name} ({client.Phone})" : "Не найдено";
        
            return Page();
        }
    }
}