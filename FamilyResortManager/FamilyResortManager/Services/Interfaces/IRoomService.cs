// File: Server/Services/Interfaces/IRoomService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using FamilyResortManager.Services.DTOs;

namespace FamilyResortManager.Services.Interfaces
{
    public interface IRoomService
    {
        Task<RoomResponseDto> GetByIdAsync(int id);
        Task<IEnumerable<RoomResponseDto>> GetAllAsync();
        Task<PagedResponseDto<RoomResponseDto>> QueryAsync(RoomQueryDto query);
        Task<RoomResponseDto> CreateAsync(RoomCreateRequestDto request);
        Task<RoomResponseDto> UpdateAsync(RoomUpdateRequestDto request);
        Task DeleteAsync(int id);
        Task BulkDeleteAsync(BulkDeleteRoomsRequestDto request);
    }
}