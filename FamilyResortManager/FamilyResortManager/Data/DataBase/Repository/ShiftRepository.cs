using FamilyResortManager.Data.DataBase.Interface;
using FamilyResortManager.Data.DataBase.Models;
using FamilyResortManager.Services.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FamilyResortManager.Data.DataBase.Repository;

public class ShiftRepository : IShiftRepository
{
    private readonly AppDbContext _context;
    public ShiftRepository(AppDbContext context) => _context = context;

    public async Task<ShiftResponseDto> GetByIdAsync(int id)
    {
        var e = await _context.Shifts.FindAsync(id) ?? throw new KeyNotFoundException();
        return Map(e);
    }

    public async Task<IEnumerable<ShiftResponseDto>> GetAllAsync()
        => (await _context.Shifts.ToListAsync()).Select(Map);

    public async Task<PagedResponseDto<ShiftResponseDto>> QueryAsync(ShiftQueryDto q)
    {
        var query = _context.Shifts.AsQueryable();
        if (q.StaffId.HasValue) query = query.Where(x => x.StaffId == q.StaffId.Value);
        if (q.DateFrom.HasValue) query = query.Where(x => x.ShiftDate >= q.DateFrom.Value);
        if (q.DateTo.HasValue) query = query.Where(x => x.ShiftDate <= q.DateTo.Value);
        var total = await query.CountAsync();
        var items = await query.Skip((q.PageNumber - 1) * q.PageSize).Take(q.PageSize).ToListAsync();
        return new PagedResponseDto<ShiftResponseDto> { Items = items.Select(Map), TotalCount = total, PageNumber = q.PageNumber, PageSize = q.PageSize };
    }

    public async Task<ShiftResponseDto> CreateAsync(ShiftCreateRequestDto r)
    {
        var e = new Shift { StaffId = r.StaffId, ShiftDate = r.ShiftDate, StartTime = r.StartTime, EndTime = r.EndTime };
        _context.Shifts.Add(e);
        await _context.SaveChangesAsync();
        return Map(e);
    }

    public async Task<ShiftResponseDto> UpdateAsync(ShiftUpdateRequestDto r)
    {
        var e = await _context.Shifts.FindAsync(r.Id) ?? throw new KeyNotFoundException();
        e.StaffId = r.StaffId; e.ShiftDate = r.ShiftDate; e.StartTime = r.StartTime; e.EndTime = r.EndTime;
        await _context.SaveChangesAsync();
        return Map(e);
    }

    public async Task DeleteAsync(int id)
    {
        var e = await _context.Shifts.FindAsync(id) ?? throw new KeyNotFoundException();
        _context.Shifts.Remove(e);
        await _context.SaveChangesAsync();
    }

    public async Task BulkDeleteAsync(BulkDeleteShiftsRequestDto req)
    {
        var items = _context.Shifts.Where(x => req.Ids.Contains(x.Id));
        _context.Shifts.RemoveRange(items);
        await _context.SaveChangesAsync();
    }

    private static ShiftResponseDto Map(Shift x)
        => new ShiftResponseDto { Id = x.Id, StaffId = x.StaffId, ShiftDate = x.ShiftDate, StartTime = x.StartTime, EndTime = x.EndTime };
}