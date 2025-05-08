using FamilyResortManager.Data.DataBase; using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Mvc; using Microsoft.AspNetCore.Mvc.RazorPages; using Microsoft.EntityFrameworkCore; using System; using System.Collections.Generic; using System.Globalization; using System.Linq; using System.Threading.Tasks;

namespace FamilyResortManager.Pages.Admin
{
    [Authorize(Policy = "RequireAdministratorRole")]
    public class AnalyticsModel : PageModel
    {
        private readonly AppDbContext _dbContext;

        public AnalyticsModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Filter properties
        [BindProperty(SupportsGet = true)] public DateTime? StartDate { get; set; }
        [BindProperty(SupportsGet = true)] public DateTime? EndDate { get; set; }
        [BindProperty(SupportsGet = true)] public string SelectedRoomType { get; set; }

        public List<string> AvailableRoomTypes { get; } = new()
        {
            "Стандарт", "Family стандарт", "Комфорт", "Family комфорт", "Полулюкс", "Люкс", "Коттедж"
        };

        // Metrics
        public int TotalBookings { get; set; }
        public int PeriodBookings { get; set; }
        public decimal PeriodIncome { get; set; }
        public List<PopularRoomViewModel> PopularRooms { get; set; } = new();

        // Chart data
        public object BookingsChartData { get; set; }
        public object RoomsChartData { get; set; }
        public object IncomeChartData { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Normalize dates to UTC
            var now = DateTime.UtcNow;
            if (!StartDate.HasValue)
            {
                StartDate = now.AddMonths(-6).Date; // Default to 6 months ago
            }
            else
            {
                // Parse date as UTC
                StartDate = DateTime.SpecifyKind(StartDate.Value.Date, DateTimeKind.Utc);
            }

            if (!EndDate.HasValue)
            {
                EndDate = now.Date; // Default to today
            }
            else
            {
                // Parse date as UTC
                EndDate = DateTime.SpecifyKind(EndDate.Value.Date, DateTimeKind.Utc);
            }

            // Validate date range
            if (EndDate < StartDate)
            {
                ModelState.AddModelError("EndDate", "Дата окончания не может быть раньше даты начала.");
                return Page();
            }

            // Total bookings
            TotalBookings = await _dbContext.Bookings.CountAsync();

            // Base query with date filter
            var bookingsQuery = _dbContext.Bookings
                .Where(b => b.CheckInDate >= StartDate && b.CheckInDate <= EndDate);

            // Apply room type filter
            if (!string.IsNullOrEmpty(SelectedRoomType))
            {
                bookingsQuery = bookingsQuery
                    .Join(_dbContext.Rooms,
                        b => b.RoomId,
                        r => r.Id,
                        (b, r) => new { Booking = b, Room = r })
                    .Where(br => br.Room.Type == SelectedRoomType)
                    .Select(br => br.Booking);
            }

            // Period bookings
            PeriodBookings = await bookingsQuery.CountAsync();

            // Period income
            PeriodIncome = await bookingsQuery
                .Join(_dbContext.Rooms,
                    b => b.RoomId,
                    r => r.Id,
                    (b, r) => (decimal)r.Price * (b.CheckOutDate - b.CheckInDate).Days)
                .SumAsync();

            // Popular rooms
            var bookingCounts = await bookingsQuery
                .GroupBy(b => b.RoomId)
                .Select(g => new { RoomId = g.Key, Count = g.Count() })
                .ToListAsync();

            var rooms = await _dbContext.Rooms.ToListAsync();

            PopularRooms = rooms
                .Select(r => new PopularRoomViewModel
                {
                    Id = r.Id,
                    Name = r.Number,
                    Type = r.Type,
                    BookingsCount = bookingCounts.FirstOrDefault(bc => bc.RoomId == r.Id)?.Count ?? 0
                })
                .OrderByDescending(r => r.BookingsCount)
                .Take(5)
                .ToList();

            // Prepare chart data
            await PrepareChartDataAsync();

            return Page();
        }

        private async Task PrepareChartDataAsync()
        {
            // Bookings chart (monthly)
            var bookingLabels = new List<string>();
            var bookingData = new List<int>();

            var currentDate = StartDate.Value;
            while (currentDate <= EndDate)
            {
                var monthEnd = currentDate.AddMonths(1).AddDays(-1) > EndDate
                    ? EndDate.Value
                    : currentDate.AddMonths(1).AddDays(-1);
                bookingLabels.Add(currentDate.ToString("MMM yyyy", new CultureInfo("ru-RU")));

                var query = _dbContext.Bookings
                    .Where(b => b.CheckInDate >= currentDate && b.CheckInDate <= monthEnd);

                if (!string.IsNullOrEmpty(SelectedRoomType))
                {
                    query = query
                        .Join(_dbContext.Rooms,
                            b => b.RoomId,
                            r => r.Id,
                            (b, r) => new { Booking = b, Room = r })
                        .Where(br => br.Room.Type == SelectedRoomType)
                        .Select(br => br.Booking);
                }

                var count = await query.CountAsync();
                bookingData.Add(count);

                currentDate = currentDate.AddMonths(1);
            }

            BookingsChartData = new
            {
                labels = bookingLabels,
                datasets = new[]
                {
                    new
                    {
                        label = "Бронирования",
                        data = bookingData,
                        borderColor = "#4BC0C0",
                        backgroundColor = "rgba(75, 192, 192, 0.5)",
                        fill = true,
                        tension = 0.4
                    }
                }
            };

            // Popular rooms chart
            RoomsChartData = new
            {
                labels = PopularRooms.Select(r => $"{r.Name} ({r.Type})").ToList(),
                datasets = new[]
                {
                    new
                    {
                        label = "Бронирования",
                        data = PopularRooms.Select(r => r.BookingsCount).ToList(),
                        backgroundColor = new[] { "#FF6384", "#36A2EB", "#FFCE56", "#4BC0C0", "#9966FF" },
                        hoverOffset = 30
                    }
                }
            };

            // Income chart (monthly)
            var incomeLabels = new List<string>();
            var incomeData = new List<decimal>();

            currentDate = StartDate.Value;
            while (currentDate <= EndDate)
            {
                var monthEnd = currentDate.AddMonths(1).AddDays(-1) > EndDate
                    ? EndDate.Value
                    : currentDate.AddMonths(1).AddDays(-1);
                incomeLabels.Add(currentDate.ToString("MMM yyyy", new CultureInfo("ru-RU")));

                var query = _dbContext.Bookings
                    .Where(b => b.CheckInDate >= currentDate && b.CheckInDate <= monthEnd);

                if (!string.IsNullOrEmpty(SelectedRoomType))
                {
                    query = query
                        .Join(_dbContext.Rooms,
                            b => b.RoomId,
                            r => r.Id,
                            (b, r) => new { Booking = b, Room = r })
                        .Where(br => br.Room.Type == SelectedRoomType)
                        .Select(br => br.Booking);
                }

                var income = await query
                    .Join(_dbContext.Rooms,
                        b => b.RoomId,
                        r => r.Id,
                        (b, r) => (decimal?)r.Price * (b.CheckOutDate - b.CheckInDate).Days)
                    .SumAsync() ?? 0m;
                incomeData.Add(income);

                currentDate = currentDate.AddMonths(1);
            }

            IncomeChartData = new
            {
                labels = incomeLabels,
                datasets = new[]
                {
                    new
                    {
                        label = "Доход (₽)",
                        data = incomeData,
                        backgroundColor = "rgba(54, 162, 235, 0.8)",
                        borderColor = "#36A2EB",
                        borderWidth = 1
                    }
                }
            };
        }
    }

    public class PopularRoomViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int BookingsCount { get; set; }
    }
}