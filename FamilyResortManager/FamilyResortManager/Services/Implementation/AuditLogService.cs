using FamilyResortManager.Data.DataBase.Interface;
using FamilyResortManager.Services.DTOs;
using FamilyResortManager.Services.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace FamilyResortManager.Services.Implementation;

public class AuditLogService : IAuditLogService
{
    private readonly IAuditLogRepository _repo;
    private readonly IValidator<AuditLogCreateRequestDto> _createValidator;

    public AuditLogService(IAuditLogRepository repo,
        IValidator<AuditLogCreateRequestDto> createValidator)
    {
        _repo = repo;
        _createValidator = createValidator;
    }

    public Task<AuditLogResponseDto> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
    public Task<IEnumerable<AuditLogResponseDto>> GetAllAsync() => _repo.GetAllAsync();
    public Task<PagedResponseDto<AuditLogResponseDto>> QueryAsync(AuditLogQueryDto query) => _repo.QueryAsync(query);

    public async Task<AuditLogResponseDto> CreateAsync(AuditLogCreateRequestDto request)
    {
        ValidationResult result = await _createValidator.ValidateAsync(request);
        if (!result.IsValid) throw new ValidationException(result.Errors);
        return await _repo.CreateAsync(request);
    }

    public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    public Task BulkDeleteAsync(BulkDeleteAuditLogsRequestDto request) => _repo.BulkDeleteAsync(request);
}