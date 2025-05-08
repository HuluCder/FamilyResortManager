using FamilyResortManager.Data.DataBase.Interface;
using FamilyResortManager.Services.DTOs;
using FamilyResortManager.Services.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace FamilyResortManager.Services.Implementation;

public class StaffService : IStaffService
{
    private readonly IStaffRepository _repo;
    private readonly IValidator<StaffCreateRequestDto> _createValidator;
    private readonly IValidator<StaffUpdateRequestDto> _updateValidator;

    public StaffService(IStaffRepository repo,
        IValidator<StaffCreateRequestDto> createValidator,
        IValidator<StaffUpdateRequestDto> updateValidator)
    {
        _repo = repo;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public Task<StaffResponseDto> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
    public Task<IEnumerable<StaffResponseDto>> GetAllAsync() => _repo.GetAllAsync();
    public Task<PagedResponseDto<StaffResponseDto>> QueryAsync(StaffQueryDto query) => _repo.QueryAsync(query);

    public async Task<StaffResponseDto> CreateAsync(StaffCreateRequestDto request)
    {
        ValidationResult result = await _createValidator.ValidateAsync(request);
        if (!result.IsValid) throw new ValidationException(result.Errors);
        return await _repo.CreateAsync(request);
    }

    public async Task<StaffResponseDto> UpdateAsync(StaffUpdateRequestDto request)
    {
        ValidationResult result = await _updateValidator.ValidateAsync(request);
        if (!result.IsValid) throw new ValidationException(result.Errors);
        return await _repo.UpdateAsync(request);
    }

    public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    public Task BulkDeleteAsync(BulkDeleteStaffRequestDto request) => _repo.BulkDeleteAsync(request);
}