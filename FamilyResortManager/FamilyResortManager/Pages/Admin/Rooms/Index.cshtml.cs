using Microsoft.AspNetCore.Mvc.RazorPages;
using FamilyResortManager.Services.Interfaces;
using FamilyResortManager.Services.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FamilyResortManager.Pages.Rooms
{
    [Authorize(Roles = "Administrator")]
    public class IndexModel : PageModel
    {
        private readonly IRoomService _service;

        public IndexModel(IRoomService service)
        {
            _service = service;
        }

        public IEnumerable<RoomResponseDto> Rooms { get; set; }

        public async Task OnGetAsync()
        {
            Rooms = await _service.GetAllAsync();
        }
    }
}