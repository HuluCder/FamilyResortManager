using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using FamilyResortManager.Services.DTOs;
using FamilyResortManager.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace FamilyResortManager.Pages.Clients
{
    [Authorize(Roles = "Administrator")]
    public class DetailsModel : PageModel
    {
        private readonly IClientService _service;
        public DetailsModel(IClientService service) => _service = service;

        [BindProperty]
        public ClientResponseDto Item { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Item = await _service.GetByIdAsync(id);
            if (Item == null) return NotFound();
            return Page();
        }
    }
}