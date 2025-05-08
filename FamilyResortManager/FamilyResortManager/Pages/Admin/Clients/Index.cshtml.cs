using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using FamilyResortManager.Services.DTOs;
using FamilyResortManager.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace FamilyResortManager.Pages.Clients
{
    [Authorize(Roles = "Administrator")]
    public class IndexModel : PageModel
    {
        private readonly IClientService _service;
        public IndexModel(IClientService service) => _service = service;

        public IEnumerable<ClientResponseDto> Clients { get; set; }

        public async Task OnGetAsync()
        {
            Clients = await _service.GetAllAsync();
        }
    }
}