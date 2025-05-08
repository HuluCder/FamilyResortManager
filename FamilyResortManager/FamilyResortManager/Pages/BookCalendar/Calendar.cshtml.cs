using FamilyResortManager.Data.DataBase; 
using FamilyResortManager.Data.DataBase.Models; 
using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.Mvc.RazorPages; 
using Microsoft.EntityFrameworkCore; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Threading.Tasks;

namespace FamilyResortManager.Pages.Bookings
{
    public class CalendarModel : PageModel
    {
        private readonly AppDbContext _context;

        public CalendarModel(AppDbContext context)
        {
            _context = context;
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

        public async Task OnGetAsync(int? month, int? year, string checkInDate = null, string checkOutDate = null,
            string roomType = null)
        {
            SelectedMonth = month ?? DateTime.UtcNow.Month;
            SelectedYear = year ?? DateTime.UtcNow.Year;
            SelectedRoomType = roomType;

            StartDate = new DateTime(SelectedYear, SelectedMonth, 1, 0, 0, 0, DateTimeKind.Utc);
            EndDate = StartDate.AddMonths(1).AddDays(-1);

            // Load all rooms initially
            var allRooms = await _context.Rooms.ToListAsync();

            // Load bookings within the calendar range
            Bookings = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.Client)
                .Where(b =>
                    (b.CheckInDate <= EndDate.AddHours(23).AddMinutes(59)) &&
                    (b.CheckOutDate >= StartDate))
                .ToListAsync();

            IsAdmin = User.IsInRole("Administrator");

            PendingBookings = Bookings.Where(b => b.Status == "Ожидание").ToList();
            Bookings = Bookings.Where(b => b.Status == "Подтверждено").ToList();

            // Filter rooms based on room type and date range
            var filteredRooms = allRooms.AsQueryable();

            // Apply room type filter
            if (!string.IsNullOrEmpty(roomType))
            {
                filteredRooms = filteredRooms.Where(r => r.Type == roomType);
            }

            // Apply date range filter
            if (!string.IsNullOrEmpty(checkInDate) && !string.IsNullOrEmpty(checkOutDate) &&
                DateTime.TryParse(checkInDate, out DateTime checkIn) &&
                DateTime.TryParse(checkOutDate, out DateTime checkOut) &&
                checkOut > checkIn)
            {
                // Adjust times for check-in (13:00) and check-out (10:00)
                checkIn = checkIn.Date.AddHours(13);
                checkOut = checkOut.Date.AddHours(10);

                // Find rooms that have no overlapping bookings
                filteredRooms = filteredRooms.Where(room => !Bookings.Any(b =>
                                                                b.RoomId == room.Id &&
                                                                b.CheckInDate < checkOut &&
                                                                b.CheckOutDate > checkIn) &&
                                                            !PendingBookings.Any(b =>
                                                                b.RoomId == room.Id &&
                                                                b.CheckInDate < checkOut &&
                                                                b.CheckOutDate > checkIn));
            }

            Rooms = filteredRooms.ToList();
        }
    }
}