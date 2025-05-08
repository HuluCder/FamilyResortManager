using FamilyResortManager.Data.DataBase.Interface;
using FamilyResortManager.Data.DataBase.Models;
using FamilyResortManager.Services.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FamilyResortManager.Data.DataBase.Repository
{
    #region Room Repository

    public class RoomRepository : IRoomRepository
    {
        private readonly AppDbContext _context;
        public RoomRepository(AppDbContext context) => _context = context;

        public async Task<RoomResponseDto> GetByIdAsync(int id)
        {
            var r = await _context.Rooms.FindAsync(id);
            return r == null ? throw new KeyNotFoundException() : Map(r);
        }

        public async Task<RoomResponseDto> GetByNumberAsync(string number)
        {
            var r = await _context.Rooms.FirstOrDefaultAsync(x => x.Number == number);
            return r == null ? throw new KeyNotFoundException() : Map(r);
        }

        public async Task<IEnumerable<RoomResponseDto>> GetAllAsync()
        {
            return (await _context.Rooms.ToListAsync()).Select(Map);
        }

        public async Task<PagedResponseDto<RoomResponseDto>> QueryAsync(RoomQueryDto q)
        {
            var query = _context.Rooms.AsQueryable();
            if (!string.IsNullOrEmpty(q.Number)) query = query.Where(x => x.Number.Contains(q.Number));
            if (!string.IsNullOrEmpty(q.Type)) query = query.Where(x => x.Type == q.Type);
            if (q.MinPrice.HasValue) query = query.Where(x => x.Price >= q.MinPrice.Value);
            if (q.MaxPrice.HasValue) query = query.Where(x => x.Price <= q.MaxPrice.Value);
            if (!string.IsNullOrEmpty(q.Status)) query = query.Where(x => x.Status == q.Status);
            var total = await query.CountAsync();
            var items = await query.Skip((q.PageNumber - 1) * q.PageSize).Take(q.PageSize).ToListAsync();
            return new PagedResponseDto<RoomResponseDto>
            {
                Items = items.Select(Map),
                TotalCount = total,
                PageNumber = q.PageNumber,
                PageSize = q.PageSize
            };
        }

        public async Task<RoomResponseDto> CreateAsync(RoomCreateRequestDto r)
        {
            var entity = new Room { Number = r.Number, Type = r.Type, Price = r.Price, Status = r.Status };
            _context.Rooms.Add(entity);
            await _context.SaveChangesAsync();
            return Map(entity);
        }

        public async Task<RoomResponseDto> UpdateAsync(RoomUpdateRequestDto r)
        {
            var entity = await _context.Rooms.FindAsync(r.Id) ?? throw new KeyNotFoundException();
            entity.Number = r.Number;
            entity.Type = r.Type;
            entity.Price = r.Price;
            entity.Status = r.Status;
            await _context.SaveChangesAsync();
            return Map(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _context.Rooms.FindAsync(id) ?? throw new KeyNotFoundException();
            _context.Rooms.Remove(e);
            await _context.SaveChangesAsync();
        }

        public async Task BulkDeleteAsync(BulkDeleteRoomsRequestDto req)
        {
            var items = _context.Rooms.Where(x => req.Ids.Contains(x.Id));
            _context.Rooms.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        private static RoomResponseDto Map(Room x)
            => new RoomResponseDto { Id = x.Id, Number = x.Number, Type = x.Type, Price = x.Price, Status = x.Status };
    }
    #endregion

    #region Client Repository

    #endregion

    // По аналогии можно добавить BookingRepository, StaffRepository, ShiftRepository, CleaningTaskRepository, NotificationRepository, AuditLogRepository
}