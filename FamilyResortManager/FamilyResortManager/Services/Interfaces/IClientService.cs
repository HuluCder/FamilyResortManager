using System.Collections.Generic;
using System.Threading.Tasks;
using FamilyResortManager.Services.DTOs;

namespace FamilyResortManager.Services.Interfaces
{
    public interface IClientService
    {
        Task<ClientResponseDto> GetByIdAsync(int id);
        Task<IEnumerable<ClientResponseDto>> GetAllAsync();
        Task<PagedResponseDto<ClientResponseDto>> QueryAsync(ClientQueryDto query);
        Task<ClientResponseDto> CreateAsync(ClientCreateRequestDto request);
        Task<ClientResponseDto> UpdateAsync(ClientUpdateRequestDto request);
        Task DeleteAsync(int id);
        Task BulkDeleteAsync(BulkDeleteClientsRequestDto request);
    }
}