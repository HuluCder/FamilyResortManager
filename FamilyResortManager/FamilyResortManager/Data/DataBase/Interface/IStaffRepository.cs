using FamilyResortManager.Services.DTOs;

namespace FamilyResortManager.Data.DataBase.Interface;

public interface IStaffRepository
{
    Task<StaffResponseDto> GetByIdAsync(int id);
    Task<IEnumerable<StaffResponseDto>> GetAllAsync();
    Task<PagedResponseDto<StaffResponseDto>> QueryAsync(StaffQueryDto query);
    Task<StaffResponseDto> CreateAsync(StaffCreateRequestDto request);
    Task<StaffResponseDto> UpdateAsync(StaffUpdateRequestDto request);
    Task DeleteAsync(int id);
    Task BulkDeleteAsync(BulkDeleteStaffRequestDto request);
}