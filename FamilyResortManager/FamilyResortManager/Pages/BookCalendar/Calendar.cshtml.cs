using FamilyResortManager.Data.DataBase;
using FamilyResortManager.Data.DataBase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FamilyResortManager.Pages.Bookings
{
    public class CalendarModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CalendarModel> _logger;
        public CalendarModel(AppDbContext context, ILogger<CalendarModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public bool IsAdmin { get; set; }
        public List<Booking> PendingBookings { get; set; } = new List<Booking>();
        public List<Room> Rooms { get; set; } = new List<Room>();
        public List<Booking> Bookings { get; set; } = new List<Booking>();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int SelectedMonth { get; set; }
        public int SelectedYear { get; set; }
        public string SelectedRoomType { get; set; }
        public string SelectedBookingStatus { get; set; }

        public async Task<IActionResult> OnGetAsync(int? month, int? year, string checkInDate = null, string checkOutDate = null, string roomType = null, string bookingStatus = null)
        {
            var minAllowed = new DateTime(2025, 6, 15, 0, 0, 0, DateTimeKind.Utc);
            var maxAllowed = new DateTime(2025, 8, 12, 0, 0, 0, DateTimeKind.Utc);

            if (!string.IsNullOrEmpty(checkInDate) && !string.IsNullOrEmpty(checkOutDate) &&
                DateTime.TryParse(checkInDate, out var checkIn) &&
                DateTime.TryParse(checkOutDate, out var checkOut))
            {
                if (checkIn < minAllowed || checkOut > maxAllowed)
                {
                    return BadRequest("Бронирование доступно только с 15 июня по 12 августа.");
                }
            }

            await LoadCalendarDataAsync(month, year, checkInDate, checkOutDate, roomType, bookingStatus);
            return Page();
        }

        // Новый метод для обработки AJAX-запросов
        public async Task<IActionResult> OnGetCalendarPartialAsync(int? month, int? year, string checkInDate = null, string checkOutDate = null, string roomType = null, string bookingStatus = null)
        {
            await LoadCalendarDataAsync(month, year, checkInDate, checkOutDate, roomType, bookingStatus);
            return Partial("_CalendarPartial", this);
        }

        private async Task LoadCalendarDataAsync(int? month, int? year, string checkInDate, string checkOutDate, string roomType, string bookingStatus)
        {
            var today = DateTime.UtcNow;
            var seasonStart = new DateTime(2025, 6, 15);
            var seasonEnd = new DateTime(2025, 8, 12);

            if (!month.HasValue || !year.HasValue)
            {
                if (today >= seasonStart && today <= seasonEnd)
                {
                    SelectedMonth = today.Month;
                    SelectedYear = today.Year;
                }
                else
                {
                    SelectedMonth = 6;
                    SelectedYear = 2025;
                }
            }
            else
            {
                SelectedMonth = month.Value;
                SelectedYear = year.Value;
            }
            SelectedRoomType = roomType;
            SelectedBookingStatus = bookingStatus;

            StartDate = new DateTime(SelectedYear, SelectedMonth, 1, 0, 0, 0, DateTimeKind.Utc);
            EndDate = StartDate.AddMonths(1).AddDays(-1);

            // Load all rooms
            var allRooms = await _context.Rooms.ToListAsync();

            // Load bookings within the calendar range, excluding cancelled
            var bookingsQuery = _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.Client)
                .Where(b =>
                    b.Status != "Отменено" &&
                    b.CheckInDate <= EndDate.AddDays(1).AddHours(10) &&
                    b.CheckOutDate >= StartDate.AddHours(13));

            if (!string.IsNullOrEmpty(bookingStatus))
            {
                bookingsQuery = bookingsQuery.Where(b => b.Status == bookingStatus);
            }

            Bookings = await bookingsQuery.Where(b => b.Status == "Подтверждено").ToListAsync();
            PendingBookings = await bookingsQuery.Where(b => b.Status == "Ожидание").ToListAsync();

            IsAdmin = User.IsInRole("Administrator");

            // Filter rooms based on room type and date range
            var filteredRooms = allRooms.AsQueryable();

            if (!string.IsNullOrEmpty(roomType))
            {
                filteredRooms = filteredRooms.Where(r => r.Type == roomType);
            }

            if (!string.IsNullOrEmpty(checkInDate) && !string.IsNullOrEmpty(checkOutDate) &&
                DateTime.TryParse(checkInDate, out DateTime checkIn) &&
                DateTime.TryParse(checkOutDate, out DateTime checkOut) &&
                checkOut > checkIn)
            {
                checkIn = checkIn.Date.AddHours(13);
                checkOut = checkOut.Date.AddHours(10);

                filteredRooms = filteredRooms.Where(room => !Bookings.Any(b =>
                    b.RoomId == room.Id &&
                    b.CheckInDate < checkOut && b.CheckOutDate > checkIn) &&
                    !PendingBookings.Any(b =>
                        b.RoomId == room.Id &&
                        b.CheckInDate < checkOut && b.CheckOutDate > checkIn));
            }

            Rooms = filteredRooms.OrderBy(r => r.Number).ToList();
        }
        
        public async Task<IActionResult> OnGetPartialAsync(int? month, int? year, string checkInDate = null, string checkOutDate = null, string roomType = null, string bookingStatus = null)
        {
            try
            {
                await OnGetAsync(month, year, checkInDate, checkOutDate, roomType, bookingStatus);
                return Partial("_CalendarPartial", this); // файл лежит в Pages/Shared
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке календаря");
                return BadRequest("Ошибка при загрузке данных календаря.");
            }
        }
        
        public IActionResult OnGetExport(int month, int year)
        {
            var startDate = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var bookings = _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.Client)
                .Where(b =>
                    b.Status != "Отменено" &&
                    b.CheckInDate <= endDate.AddDays(1).AddHours(10) &&
                    b.CheckOutDate >= startDate.AddHours(13))
                .ToList();

            var csv = "Номер,Тип номера,Клиент,Дата заезда,Дата выезда,Статус\n";
            foreach (var booking in bookings)
            {
                csv += $"{booking.Room.Number},{booking.Room.Type},{booking.Client?.Name ?? ""},{booking.CheckInDate:dd.MM.yyyy HH:mm},{booking.CheckOutDate:dd.MM.yyyy HH:mm},{booking.Status}\n";
            }

            return File(System.Text.Encoding.UTF8.GetBytes(csv), "text/csv", $"bookings_{month}_{year}.csv");
        }
    }
}