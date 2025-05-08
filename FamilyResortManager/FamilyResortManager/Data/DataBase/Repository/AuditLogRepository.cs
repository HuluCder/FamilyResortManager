using FamilyResortManager.Data.DataBase.Interface;
using FamilyResortManager.Data.DataBase.Models;
using FamilyResortManager.Services.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FamilyResortManager.Data.DataBase.Repository;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly AppDbContext _context;
    public AuditLogRepository(AppDbContext context) => _context = context;

    public async Task<AuditLogResponseDto> GetByIdAsync(int id)
    {
        var e = await _context.AuditLog.FindAsync(id) ?? throw new KeyNotFoundException();
        return Map(e);
    }

    public async Task<IEnumerable<AuditLogResponseDto>> GetAllAsync()
        => (await _context.AuditLog.ToListAsync()).Select(Map);

    public async Task<PagedResponseDto<AuditLogResponseDto>> QueryAsync(PageRequestDto q)
    {
        var query = _context.AuditLog.AsQueryable();
        var total = await query.CountAsync();
        var items = await query.Skip((q.PageNumber - 1) * q.PageSize).Take(q.PageSize).ToListAsync();
        return new PagedResponseDto<AuditLogResponseDto> { Items = items.Select(Map), TotalCount = total, PageNumber = q.PageNumber, PageSize = q.PageSize };
    }

    public async Task<AuditLogResponseDto> CreateAsync(AuditLogCreateRequestDto r)
    {
        var e = new AuditLog { Action = r.Action, Details = r.Details, CreatedAt = r.CreatedAt };
        _context.AuditLog.Add(e);
        await _context.SaveChangesAsync();
        return Map(e);
    }

    public async Task DeleteAsync(int id)
    {
        var e = await _context.AuditLog.FindAsync(id) ?? throw new KeyNotFoundException();
        _context.AuditLog.Remove(e);
        await _context.SaveChangesAsync();
    }

    public async Task BulkDeleteAsync(BulkDeleteAuditLogsRequestDto req)
    {
        var items = _context.AuditLog.Where(x => req.Ids.Contains(x.Id));
        _context.AuditLog.RemoveRange(items);
        await _context.SaveChangesAsync();
    }

    private static AuditLogResponseDto Map(AuditLog x)
        => new AuditLogResponseDto { Id = x.Id, Action = x.Action, Details = x.Details, CreatedAt = x.CreatedAt };
}