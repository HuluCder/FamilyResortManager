using FamilyResortManager.Data.DataBase.Interface;
using FamilyResortManager.Data.DataBase.Models;
using FamilyResortManager.Services.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FamilyResortManager.Data.DataBase.Repository;

public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext _context;
    public NotificationRepository(AppDbContext context) => _context = context;

    public async Task<NotificationResponseDto> GetByIdAsync(int id)
    {
        var e = await _context.Notifications.FindAsync(id) ?? throw new KeyNotFoundException();
        return Map(e);
    }

    public async Task<IEnumerable<NotificationResponseDto>> GetAllAsync()
        => (await _context.Notifications.ToListAsync()).Select(Map);

    public async Task<PagedResponseDto<NotificationResponseDto>> QueryAsync(PageRequestDto q)
    {
        var query = _context.Notifications.AsQueryable();
        var total = await query.CountAsync();
        var items = await query.Skip((q.PageNumber - 1) * q.PageSize).Take(q.PageSize).ToListAsync();
        return new PagedResponseDto<NotificationResponseDto> { Items = items.Select(Map), TotalCount = total, PageNumber = q.PageNumber, PageSize = q.PageSize };
    }

    public async Task<NotificationResponseDto> CreateAsync(NotificationCreateRequestDto r)
    {
        var e = new Notification { Message = r.Message, CreatedAt = r.CreatedAt };
        _context.Notifications.Add(e);
        await _context.SaveChangesAsync();
        return Map(e);
    }

    public async Task<NotificationResponseDto> UpdateAsync(NotificationUpdateRequestDto r)
    {
        var e = await _context.Notifications.FindAsync(r.Id) ?? throw new KeyNotFoundException();
        e.Message = r.Message; e.CreatedAt = r.CreatedAt;
        await _context.SaveChangesAsync();
        return Map(e);
    }

    public async Task DeleteAsync(int id)
    {
        var e = await _context.Notifications.FindAsync(id) ?? throw new KeyNotFoundException();
        _context.Notifications.Remove(e);
        await _context.SaveChangesAsync();
    }

    public async Task BulkDeleteAsync(BulkDeleteNotificationsRequestDto req)
    {
        var items = _context.Notifications.Where(x => req.Ids.Contains(x.Id));
        _context.Notifications.RemoveRange(items);
        await _context.SaveChangesAsync();
    }

    private static NotificationResponseDto Map(Notification x)
        => new NotificationResponseDto { Id = x.Id, Message = x.Message, CreatedAt = x.CreatedAt };
}