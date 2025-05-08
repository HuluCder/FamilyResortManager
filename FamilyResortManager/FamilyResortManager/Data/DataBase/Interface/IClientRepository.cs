using FamilyResortManager.Services.DTOs;

namespace FamilyResortManager.Data.DataBase.Interface;

public interface IClientRepository
{
    Task<ClientResponseDto> GetByIdAsync(int id);
    Task<ClientResponseDto> GetByNameAsync(string name);
    Task<IEnumerable<ClientResponseDto>> GetAllAsync();
    Task<PagedResponseDto<ClientResponseDto>> QueryAsync(ClientQueryDto query);
    Task<ClientResponseDto> CreateAsync(ClientCreateRequestDto req);
    Task<ClientResponseDto> UpdateAsync(ClientUpdateRequestDto req);
    Task DeleteAsync(int id);
    Task BulkDeleteAsync(BulkDeleteClientsRequestDto req);
}