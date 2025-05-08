using FamilyResortManager.Data.DataBase;
using FamilyResortManager.Data.DataBase.Models;
using Microsoft.AspNetCore.Mvc;

namespace FamilyResortManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookingsController(AppDbContext context)
        {
            _context = context;
        }

        public class BookingRequest
        {
            public int RoomId { get; set; }
            public DateTime CheckInDate { get; set; }
            public DateTime CheckOutDate { get; set; }
            public ClientRequest Client { get; set; }
        }

        public class ClientRequest
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var room = await _context.Rooms.FindAsync(request.RoomId);
            if (room == null)
            {
                return BadRequest("Указанный номер не найден");
            }

            // Проверка доступности номера на указанные даты
            var existingBookings = _context.Bookings
                .Where(b => b.RoomId == request.RoomId &&
                          ((b.CheckInDate <= request.CheckInDate && b.CheckOutDate >= request.CheckInDate) ||
                           (b.CheckInDate <= request.CheckOutDate && b.CheckOutDate >= request.CheckOutDate) ||
                           (b.CheckInDate >= request.CheckInDate && b.CheckOutDate <= request.CheckOutDate)))
                .ToList();

            if (existingBookings.Any())
            {
                return BadRequest("Номер уже забронирован на указанные даты");
            }

            // Создаем нового клиента
            var client = new Client
            {
                Name = request.Client.Name,
                Email = request.Client.Email,
                Phone = request.Client.Phone
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            // Создаем новое бронирование
            var booking = new Booking
            {
                RoomId = request.RoomId,
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                ClientId = client.Id,
                Status = "Ожидание"
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}