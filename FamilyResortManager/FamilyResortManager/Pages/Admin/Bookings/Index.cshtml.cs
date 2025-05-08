using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using FamilyResortManager.Services.DTOs;
using FamilyResortManager.Services.Interfaces;

namespace FamilyResortManager.Pages.Bookings
{
    public class IndexModel : PageModel
    {
        private readonly IBookingService _service;
        public IndexModel(IBookingService service) => _service = service;

        public IEnumerable<BookingResponseDto> Bookings { get; set; }

        public async Task OnGetAsync()
        {
            Bookings = await _service.GetAllAsync();
        }
    }
}