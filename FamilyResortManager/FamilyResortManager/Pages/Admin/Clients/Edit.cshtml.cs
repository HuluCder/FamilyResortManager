using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using FamilyResortManager.Services.DTOs;
using FamilyResortManager.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace FamilyResortManager.Pages.Clients
{
    [Authorize(Roles = "Administrator")]
    public class EditModel : PageModel
    {
        private readonly IClientService _service;
        public EditModel(IClientService service) => _service = service;

        [BindProperty]
        public ClientUpdateRequestDto Input { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = await _service.GetByIdAsync(id);
            if (client == null) return NotFound();

            Input = new ClientUpdateRequestDto
            {
                Id    = client.Id,
                Name  = client.Name,
                Email = client.Email,
                Phone = client.Phone,
                Info  = client.Info
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