using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FamilyResortManager.Services.Interfaces;
using FamilyResortManager.Services.DTOs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FamilyResortManager.Pages.Rooms
{
    [Authorize(Roles = "Administrator")]
    public class DeleteModel : PageModel
    {
        private readonly IRoomService _service;

        public DeleteModel(IRoomService service)
        {
            _service = service;
        }

        // На эту модель мы привяжем скрытое поле с Id
        [BindProperty]
        public RoomResponseDto Item { get; set; }

        // Загрузим данные для показа пользователю
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Item = await _service.GetByIdAsync(id);
            if (Item == null)
                return NotFound();

            return Page();
        }

        // При POST удаляем и возвращаемся на список
        public async Task<IActionResult> OnPostAsync()
        {
            await _service.DeleteAsync(Item.Id);
            return RedirectToPage("./Index");
        }
    }
}