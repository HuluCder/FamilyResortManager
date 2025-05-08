using FamilyResortManager.Data.DataBase.Interface;
using FamilyResortManager.Data.DataBase.Models;
using FamilyResortManager.Services.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FamilyResortManager.Data.DataBase.Repository;

public class CleaningTaskRepository : ICleaningTaskRepository
{
    private readonly AppDbContext _context;
    public CleaningTaskRepository(AppDbContext context) => _context = context;

    public async Task<CleaningTaskResponseDto> GetByIdAsync(int id)
    {
        var e = await _context.CleaningTasks.FindAsync(id) ?? throw new KeyNotFoundException();
        return Map(e);
    }

    public async Task<IEnumerable<CleaningTaskResponseDto>> GetAllAsync()
        => (await _context.CleaningTasks.ToListAsync()).Select(Map);

    public async Task<PagedResponseDto<CleaningTaskResponseDto>> QueryAsync(CleaningTaskQueryDto q)
    {
        var query = _context.CleaningTasks.AsQueryable();
        if (q.RoomId.HasValue) query = query.Where(x => x.RoomId == q.RoomId.Value);
        if (q.StaffId.HasValue) query = query.Where(x => x.StaffId == q.StaffId.Value);
        if (q.DateFrom.HasValue) query = query.Where(x => x.TaskDate >= q.DateFrom.Value);
        if (q.DateTo.HasValue) query = query.Where(x => x.TaskDate <= q.DateTo.Value);
        if (!string.IsNullOrEmpty(q.Status)) query = query.Where(x => x.Status == q.Status);
        var total = await query.CountAsync();
        var items = await query.Skip((q.PageNumber - 1) * q.PageSize).Take(q.PageSize).ToListAsync();
        return new PagedResponseDto<CleaningTaskResponseDto> { Items = items.Select(Map), TotalCount = total, PageNumber = q.PageNumber, PageSize = q.PageSize };
    }

    public async Task<CleaningTaskResponseDto> CreateAsync(CleaningTaskCreateRequestDto r)
    {
        var e = new CleaningTask { RoomId = r.RoomId, StaffId = r.StaffId, TaskDate = r.TaskDate, Status = r.Status };  
        _context.CleaningTasks.Add(e);
        await _context.SaveChangesAsync();
        return Map(e);
    }

    public async Task<CleaningTaskResponseDto> UpdateAsync(CleaningTaskUpdateRequestDto r)
    {
        var e = await _context.CleaningTasks.FindAsync(r.Id) ?? throw new KeyNotFoundException();
        e.RoomId = r.RoomId; e.StaffId = r.StaffId; e.TaskDate = r.TaskDate; e.Status = r.Status;
        await _context.SaveChangesAsync();
        return Map(e);
    }

    public async Task DeleteAsync(int id)
    {
        var e = await _context.CleaningTasks.FindAsync(id) ?? throw new KeyNotFoundException();
        _context.CleaningTasks.Remove(e);
        await _context.SaveChangesAsync();
    }

    public async Task BulkDeleteAsync(BulkDeleteCleaningTasksRequestDto req)
    {
        var items = _context.CleaningTasks.Where(x => req.Ids.Contains(x.Id));
        _context.CleaningTasks.RemoveRange(items);
        await _context.SaveChangesAsync();
    }

    private static CleaningTaskResponseDto Map(CleaningTask x)
        => new CleaningTaskResponseDto { Id = x.Id, RoomId = x.RoomId, StaffId = x.StaffId, TaskDate = x.TaskDate, Status = x.Status };
}