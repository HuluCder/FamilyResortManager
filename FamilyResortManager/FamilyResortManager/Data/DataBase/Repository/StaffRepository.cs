using FamilyResortManager.Data.DataBase.Interface;
using FamilyResortManager.Data.DataBase.Models;
using FamilyResortManager.Services.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FamilyResortManager.Data.DataBase.Repository;

public class StaffRepository : IStaffRepository
{
    private readonly AppDbContext _context;
    public StaffRepository(AppDbContext context) => _context = context;

    public async Task<StaffResponseDto> GetByIdAsync(int id)
    {
        var e = await _context.Staff.FindAsync(id) ?? throw new KeyNotFoundException();
        return Map(e);
    }

    public async Task<IEnumerable<StaffResponseDto>> GetAllAsync()
        => (await _context.Staff.ToListAsync()).Select(Map);

    public async Task<PagedResponseDto<StaffResponseDto>> QueryAsync(StaffQueryDto q)
    {
        var query = _context.Staff.AsQueryable();
        if (!string.IsNullOrEmpty(q.Name)) query = query.Where(x => x.Name.Contains(q.Name));
        if (!string.IsNullOrEmpty(q.Position)) query = query.Where(x => x.Position == q.Position);
        if (!string.IsNullOrEmpty(q.Email)) query = query.Where(x => x.Email.Contains(q.Email));
        var total = await query.CountAsync();
        var items = await query.Skip((q.PageNumber - 1) * q.PageSize).Take(q.PageSize).ToListAsync();
        return new PagedResponseDto<StaffResponseDto> { Items = items.Select(Map), TotalCount = total, PageNumber = q.PageNumber, PageSize = q.PageSize };
    }

    public async Task<StaffResponseDto> CreateAsync(StaffCreateRequestDto r)
    {
        var e = new Staff { Name = r.Name, Position = r.Position, Email = r.Email, HourlyRate = r.HourlyRate };
        _context.Staff.Add(e);
        await _context.SaveChangesAsync();
        return Map(e);
    }

    public async Task<StaffResponseDto> UpdateAsync(StaffUpdateRequestDto r)
    {
        var e = await _context.Staff.FindAsync(r.Id) ?? throw new KeyNotFoundException();
        e.Name = r.Name; e.Position = r.Position; e.Email = r.Email; e.HourlyRate = r.HourlyRate;
        await _context.SaveChangesAsync();
        return Map(e);
    }

    public async Task DeleteAsync(int id)
    {
        var e = await _context.Staff.FindAsync(id) ?? throw new KeyNotFoundException();
        _context.Staff.Remove(e);
        await _context.SaveChangesAsync();
    }

    public async Task BulkDeleteAsync(BulkDeleteStaffRequestDto req)
    {
        var items = _context.Staff.Where(x => req.Ids.Contains(x.Id));
        _context.Staff.RemoveRange(items);
        await _context.SaveChangesAsync();
    }

    private static StaffResponseDto Map(Staff x)
        => new StaffResponseDto { Id = x.Id, Name = x.Name, Position = x.Position, Email = x.Email, HourlyRate = x.HourlyRate };
}