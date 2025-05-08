using FamilyResortManager.Services.DTOs;

namespace FamilyResortManager.Services.Interfaces;

public interface IShiftService
{
    Task<ShiftResponseDto> GetByIdAsync(int id);
    Task<IEnumerable<ShiftResponseDto>> GetAllAsync();
    Task<PagedResponseDto<ShiftResponseDto>> QueryAsync(ShiftQueryDto query);
    Task<ShiftResponseDto> CreateAsync(ShiftCreateRequestDto request);
    Task<ShiftResponseDto> UpdateAsync(ShiftUpdateRequestDto request);
    Task DeleteAsync(int id);
    Task BulkDeleteAsync(BulkDeleteShiftsRequestDto request);
}