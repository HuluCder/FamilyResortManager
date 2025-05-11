using FamilyResortManager.Data.DataBase;
using FamilyResortManager.Data.DataBase.Models;
using FamilyResortManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FamilyResortManager.Pages.Client.Bookings
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;

        [BindProperty] public Booking Booking { get; set; }

        public string RoomNumber { get; set; }

        public CreateModel(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<IActionResult> OnGetAsync(int roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
            {
                return NotFound();
            }

            RoomNumber = room.Number;

            // Установка времени: 13:00 для въезда, 10:00 для выезда
            checkInDate = checkInDate.Date.AddHours(13);
            checkOutDate = checkOutDate.Date.AddHours(10);

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
                // Парсинг дат с явным указанием UTC
                DateTime checkInDate = DateTime.Parse(form["Input.CheckInDate"], null,
                    System.Globalization.DateTimeStyles.AdjustToUniversal);
                DateTime checkOutDate = DateTime.Parse(form["Input.CheckOutDate"], null,
                    System.Globalization.DateTimeStyles.AdjustToUniversal);

                // Проверка корректности дат
                if (checkOutDate <= checkInDate)
                {
                    return StatusCode(400, "Дата выезда должна быть позже даты въезда.");
                }

                string clientName = form["ClientName"];
                string clientEmail = form["ClientEmail"];
                string clientPhone = form["ClientPhone"];

                // 🔒 Проверка на пересекающееся бронирование с учетом времени
                bool hasConflict = await _context.Bookings.AnyAsync(b =>
                    b.RoomId == roomId &&
                    b.Status != "Отменено" &&
                    b.CheckInDate < checkOutDate &&
                    b.CheckOutDate > checkInDate);

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

                // Получаем информацию о номере
                var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == roomId);
                var roomName = room != null ? $"№{room.Number} ({room.Type})" : $"Комната ID {roomId}";

                // 📧 Отправка письма админу
                var subject = "🔔 Новая заявка на бронирование";
                var body = $@"
            <h3>Поступила новая заявка</h3>
            <p><strong>Имя:</strong> {clientName}</p>
            <p><strong>Телефон:</strong> {clientPhone}</p>
            <p><strong>Email:</strong> {clientEmail}</p>
            <p><strong>Номер комнаты:</strong> {roomName}</p>
            <p><strong>Период:</strong> {checkInDate:dd.MM.yyyy HH:mm} – {checkOutDate:dd.MM.yyyy HH:mm}</p>
            <p><strong>Статус:</strong> Ожидание</p>
            <p><a href=""https://localhost:7058/Admin/Bookings/Details/{booking.Id}"">Перейти к заявке в админке</a></p>";

                await _emailService.SendEmailAsync("danya16f@gmail.com", subject, body);

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
