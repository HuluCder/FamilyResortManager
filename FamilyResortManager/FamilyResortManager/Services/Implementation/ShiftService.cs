using FamilyResortManager.Data.DataBase.Interface;
using FamilyResortManager.Services.DTOs;
using FamilyResortManager.Services.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace FamilyResortManager.Services.Implementation;

public class ShiftService : IShiftService
{
    private readonly IShiftRepository _repo;
    private readonly IValidator<ShiftCreateRequestDto> _createValidator;
    private readonly IValidator<ShiftUpdateRequestDto> _updateValidator;

    public ShiftService(IShiftRepository repo,
        IValidator<ShiftCreateRequestDto> createValidator,
        IValidator<ShiftUpdateRequestDto> updateValidator)
    {
        _repo = repo;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public Task<ShiftResponseDto> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
    public Task<IEnumerable<ShiftResponseDto>> GetAllAsync() => _repo.GetAllAsync();
    public Task<PagedResponseDto<ShiftResponseDto>> QueryAsync(ShiftQueryDto query) => _repo.QueryAsync(query);

    public async Task<ShiftResponseDto> CreateAsync(ShiftCreateRequestDto request)
    {
        ValidationResult result = await _createValidator.ValidateAsync(request);
        if (!result.IsValid) throw new ValidationException(result.Errors);
        return await _repo.CreateAsync(request);
    }

    public async Task<ShiftResponseDto> UpdateAsync(ShiftUpdateRequestDto request)
    {
        ValidationResult result = await _updateValidator.ValidateAsync(request);
        if (!result.IsValid) throw new ValidationException(result.Errors);
        return await _repo.UpdateAsync(request);
    }

    public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    public Task BulkDeleteAsync(BulkDeleteShiftsRequestDto request) => _repo.BulkDeleteAsync(request);
}