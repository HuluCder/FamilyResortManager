using System.Data;
using FamilyResortManager.Data.DataBase.Interface;
using FamilyResortManager.Data.DataBase.Models;
using FamilyResortManager.Services.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FamilyResortManager.Data.DataBase.Repository
{
    #region Booking Repository

    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;
        public BookingRepository(AppDbContext context) => _context = context;

        public async Task<BookingResponseDto> GetByIdAsync(int id)
        {
            var e = await _context.Bookings.FindAsync(id) ?? throw new KeyNotFoundException();
            return Map(e);
        }

        public async Task<IEnumerable<BookingResponseDto>> GetAllAsync()
            => (await _context.Bookings.ToListAsync()).Select(Map);

        public async Task<PagedResponseDto<BookingResponseDto>> QueryAsync(BookingQueryDto q)
        {
            var query = _context.Bookings.AsQueryable();
            if (q.RoomId.HasValue) query = query.Where(x => x.RoomId == q.RoomId.Value);
            if (q.ClientId.HasValue) query = query.Where(x => x.ClientId == q.ClientId.Value);
            if (q.CheckInDateFrom.HasValue) query = query.Where(x => x.CheckInDate >= q.CheckInDateFrom.Value);
            if (q.CheckInDateTo.HasValue) query = query.Where(x => x.CheckInDate <= q.CheckInDateTo.Value);
            if (!string.IsNullOrEmpty(q.Status)) query = query.Where(x => x.Status == q.Status);
            var total = await query.CountAsync();
            var items = await query.Skip((q.PageNumber - 1) * q.PageSize).Take(q.PageSize).ToListAsync();
            return new PagedResponseDto<BookingResponseDto> { Items = items.Select(Map), TotalCount = total, PageNumber = q.PageNumber, PageSize = q.PageSize };
        }

        public async Task<BookingResponseDto> CreateAsync(BookingCreateRequestDto r)
        {
            var e = new Booking { RoomId = r.RoomId, ClientId = r.ClientId, CheckInDate  = DateTime.SpecifyKind(r.CheckInDate, DateTimeKind.Utc), CheckOutDate = DateTime.SpecifyKind(r.CheckOutDate, DateTimeKind.Utc), Status = r.Status };
            _context.Bookings.Add(e);
            await _context.SaveChangesAsync();
            return Map(e);
        }

        public async Task<BookingResponseDto> UpdateAsync(BookingUpdateRequestDto r)
        {
            var e = await _context.Bookings.FindAsync(r.Id) ?? throw new KeyNotFoundException();
            e.RoomId = r.RoomId; e.ClientId = r.ClientId;
            e.CheckInDate  = DateTime.SpecifyKind(r.CheckInDate, DateTimeKind.Utc);
            e.CheckOutDate = DateTime.SpecifyKind(r.CheckOutDate, DateTimeKind.Utc);
            e.Status = r.Status;
            await _context.SaveChangesAsync();
            return Map(e);
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _context.Bookings.FindAsync(id) ?? throw new KeyNotFoundException();
            _context.Bookings.Remove(e);
            await _context.SaveChangesAsync();
        }

        public async Task BulkDeleteAsync(BulkDeleteBookingsRequestDto req)
        {
            var items = _context.Bookings.Where(x => req.Ids.Contains(x.Id));
            _context.Bookings.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        private static BookingResponseDto Map(Booking x)
            => new BookingResponseDto { Id = x.Id, RoomId = x.RoomId, ClientId = x.ClientId, CheckInDate = x.CheckInDate, CheckOutDate = x.CheckOutDate, Status = x.Status };
    }
    #endregion

    #region Staff Repository

    #endregion

    #region Shift Repository

    #endregion

    #region CleaningTask Repository

    #endregion

    #region Notification Repository

    #endregion

    #region AuditLog Repository

    #endregion
}
