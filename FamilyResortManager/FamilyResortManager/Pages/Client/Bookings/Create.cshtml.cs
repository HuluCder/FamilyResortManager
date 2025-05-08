using FamilyResortManager.Data.DataBase;
using FamilyResortManager.Data.DataBase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FamilyResortManager.Pages.Client.Bookings
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        
        [BindProperty]
        public Booking Booking { get; set; }
        
        public string RoomNumber { get; set; }
        
        public CreateModel(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> OnGetAsync(int roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
            {
                return NotFound();
            }
            
            RoomNumber = room.Number;
            
            Booking = new Booking
            {
                RoomId = roomId,
                CheckInDate = checkInDate,
                CheckOutDate = checkOutDate,
                Status = "Ожидание",
                Client = new Data.DataBase.Models.Client()
            };
            
            return Page();
        }
        
        public async Task<IActionResult> OnPostCreateBookingAsync()
{
    try
    {
        var form = Request.Form;

        int roomId = int.Parse(form["Input.RoomId"]);
        DateTime checkInDate = DateTime.SpecifyKind(
            DateTime.Parse(form["Input.CheckInDate"]), DateTimeKind.Utc);
        DateTime checkOutDate = DateTime.SpecifyKind(
            DateTime.Parse(form["Input.CheckOutDate"]), DateTimeKind.Utc);

        string clientName = form["ClientName"];
        string clientEmail = form["ClientEmail"];
        string clientPhone = form["ClientPhone"];

        // 🔒 Проверка на пересекающееся бронирование
        bool hasConflict = await _context.Bookings.AnyAsync(b =>
            b.RoomId == roomId &&
            b.Status != "Отменено" &&
            checkInDate < b.CheckOutDate &&
            checkOutDate > b.CheckInDate
        );

        if (hasConflict)
        {
            return StatusCode(409, "Выбранный период пересекается с уже существующим бронированием.");
        }

        // 👤 Поиск или создание клиента
        var existingClient = await _context.Clients.FirstOrDefaultAsync(c => c.Phone == clientPhone);
        int clientId;

        if (existingClient == null)
        {
            var newClient = new Data.DataBase.Models.Client
            {
                Name = clientName,
                Email = clientEmail,
                Phone = clientPhone,
                Info = "С сайта"
            };

            _context.Clients.Add(newClient);
            await _context.SaveChangesAsync();

            clientId = newClient.Id;
        }
        else
        {
            clientId = existingClient.Id;
        }

        // 📅 Создание бронирования
        var booking = new Booking
        {
            RoomId = roomId,
            ClientId = clientId,
            CheckInDate = checkInDate,
            CheckOutDate = checkOutDate,
            Status = "Ожидание"
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return new JsonResult(new { success = true });
    }
    catch (DbUpdateException dbEx)
    {
        var inner = dbEx.InnerException?.Message;
        Console.WriteLine("🔴 EF Error: " + dbEx.Message);
        Console.WriteLine("🔧 Inner: " + inner);

        return StatusCode(500, "Ошибка при сохранении в БД: " + inner);
    }
    catch (Exception ex)
    {
        Console.WriteLine("⚠️ Ошибка: " + ex.Message);
        return StatusCode(500, "Общая ошибка: " + ex.Message);
    }
}

    }

}