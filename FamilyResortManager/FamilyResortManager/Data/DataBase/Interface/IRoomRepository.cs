using FamilyResortManager.Services.DTOs;

namespace FamilyResortManager.Data.DataBase.Interface;

public interface IRoomRepository
{
    Task<RoomResponseDto> GetByIdAsync(int id);
    Task<RoomResponseDto> GetByNumberAsync(string number);
    Task<IEnumerable<RoomResponseDto>> GetAllAsync();
    Task<PagedResponseDto<RoomResponseDto>> QueryAsync(RoomQueryDto query);
    Task<RoomResponseDto> CreateAsync(RoomCreateRequestDto request);
    Task<RoomResponseDto> UpdateAsync(RoomUpdateRequestDto request);
    Task DeleteAsync(int id);
    Task BulkDeleteAsync(BulkDeleteRoomsRequestDto request);
}