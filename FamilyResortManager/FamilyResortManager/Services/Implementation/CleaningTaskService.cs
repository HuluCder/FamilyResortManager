using FamilyResortManager.Data.DataBase.Interface;
using FamilyResortManager.Services.DTOs;
using FamilyResortManager.Services.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace FamilyResortManager.Services.Implementation;

public class CleaningTaskService : ICleaningTaskService
{
    private readonly ICleaningTaskRepository _repo;
    private readonly IValidator<CleaningTaskCreateRequestDto> _createValidator;
    private readonly IValidator<CleaningTaskUpdateRequestDto> _updateValidator;

    public CleaningTaskService(ICleaningTaskRepository repo,
        IValidator<CleaningTaskCreateRequestDto> createValidator,
        IValidator<CleaningTaskUpdateRequestDto> updateValidator)
    {
        _repo = repo;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public Task<CleaningTaskResponseDto> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
    public Task<IEnumerable<CleaningTaskResponseDto>> GetAllAsync() => _repo.GetAllAsync();
    public Task<PagedResponseDto<CleaningTaskResponseDto>> QueryAsync(CleaningTaskQueryDto query) => _repo.QueryAsync(query);

    public async Task<CleaningTaskResponseDto> CreateAsync(CleaningTaskCreateRequestDto request)
    {
        ValidationResult result = await _createValidator.ValidateAsync(request);
        if (!result.IsValid) throw new ValidationException(result.Errors);
        return await _repo.CreateAsync(request);
    }

    public async Task<CleaningTaskResponseDto> UpdateAsync(CleaningTaskUpdateRequestDto request)
    {
        ValidationResult result = await _updateValidator.ValidateAsync(request);
        if (!result.IsValid) throw new ValidationException(result.Errors);
        return await _repo.UpdateAsync(request);
    }

    public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    public Task BulkDeleteAsync(BulkDeleteCleaningTasksRequestDto request) => _repo.BulkDeleteAsync(request);
}