using FamilyResortManager.Data.DataBase.Interface;
using FamilyResortManager.Services.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FamilyResortManager.Data.DataBase.Repository;

public class ClientRepository : IClientRepository
{
    private readonly AppDbContext _context;
    public ClientRepository(AppDbContext context) => _context = context;

    public async Task<ClientResponseDto> GetByIdAsync(int id)
    {
        var e = await _context.Clients.FindAsync(id) ?? throw new KeyNotFoundException();
        return Map(e);
    }

    public async Task<ClientResponseDto> GetByNameAsync(string name)
    {
        var e = await _context.Clients.FirstOrDefaultAsync(x => x.Name == name) ?? throw new KeyNotFoundException();
        return Map(e);
    }

    public async Task<IEnumerable<ClientResponseDto>> GetAllAsync()
        => (await _context.Clients.ToListAsync()).Select(Map);

    public async Task<PagedResponseDto<ClientResponseDto>> QueryAsync(ClientQueryDto q)
    {
        var query = _context.Clients.AsQueryable();
        if (!string.IsNullOrEmpty(q.Name)) query = query.Where(x => x.Name.Contains(q.Name));
        if (!string.IsNullOrEmpty(q.Email)) query = query.Where(x => x.Email.Contains(q.Email));
        if (!string.IsNullOrEmpty(q.Phone)) query = query.Where(x => x.Phone.Contains(q.Phone));
        var total = await query.CountAsync();
        var items = await query.Skip((q.PageNumber - 1) * q.PageSize).Take(q.PageSize).ToListAsync();
        return new PagedResponseDto<ClientResponseDto> { Items = items.Select(Map), TotalCount = total, PageNumber = q.PageNumber, PageSize = q.PageSize };
    }

    public async Task<ClientResponseDto> CreateAsync(ClientCreateRequestDto r)
    {
        var e = new Models.Client { Name = r.Name, Email = r.Email, Phone = r.Phone, Info = r.Info };
        _context.Clients.Add(e);
        await _context.SaveChangesAsync();
        return Map(e);
    }

    public async Task<ClientResponseDto> UpdateAsync(ClientUpdateRequestDto r)
    {
        var e = await _context.Clients.FindAsync(r.Id) ?? throw new KeyNotFoundException();
        e.Name = r.Name;
        e.Email = r.Email;
        e.Phone = r.Phone;
        e.Info = r.Info;
        await _context.SaveChangesAsync();
        return Map(e);
    }

    public async Task DeleteAsync(int id)
    {
        var e = await _context.Clients.FindAsync(id) ?? throw new KeyNotFoundException();
        _context.Clients.Remove(e);
        await _context.SaveChangesAsync();
    }

    public async Task BulkDeleteAsync(BulkDeleteClientsRequestDto req)
    {
        var items = _context.Clients.Where(x => req.Ids.Contains(x.Id));
        _context.Clients.RemoveRange(items);
        await _context.SaveChangesAsync();
    }

    private static ClientResponseDto Map(Models.Client x)
        => new ClientResponseDto { Id = x.Id, Name = x.Name, Email = x.Email, Phone = x.Phone, Info = x.Info };
}