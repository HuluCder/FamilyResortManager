using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using FamilyResortManager.Services.DTOs;
using FamilyResortManager.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace FamilyResortManager.Pages.Clients
{
    [Authorize(Roles = "Administrator")]
    public class CreateModel : PageModel
    {
        private readonly IClientService _service;
        public CreateModel(IClientService service) => _service = service;

        [BindProperty]
        public ClientCreateRequestDto Input { get; set; }

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