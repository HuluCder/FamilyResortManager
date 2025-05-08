using FamilyResortManager.Services.DTOs;

namespace FamilyResortManager.Data.DataBase.Interface;

public interface ICleaningTaskRepository
{
    Task<CleaningTaskResponseDto> GetByIdAsync(int id);
    Task<IEnumerable<CleaningTaskResponseDto>> GetAllAsync();
    Task<PagedResponseDto<CleaningTaskResponseDto>> QueryAsync(CleaningTaskQueryDto query);
    Task<CleaningTaskResponseDto> CreateAsync(CleaningTaskCreateRequestDto request);
    Task<CleaningTaskResponseDto> UpdateAsync(CleaningTaskUpdateRequestDto request);
    Task DeleteAsync(int id);
    Task BulkDeleteAsync(BulkDeleteCleaningTasksRequestDto request);
}