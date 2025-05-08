using FamilyResortManager.Data.DataBase.Interface;
using FamilyResortManager.Services.DTOs;
using FamilyResortManager.Services.Interfaces;
using FamilyResortManager.Services.Validators;
using FluentValidation;
using FluentValidation.Results;

namespace FamilyResortManager.Services.Implementation;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repo;
    private readonly IValidator<NotificationCreateRequestDto> _createValidator;
    private readonly IValidator<NotificationUpdateRequestDto> _updateValidator;

    public NotificationService(INotificationRepository repo,
        IValidator<NotificationCreateRequestDto> createValidator,
        IValidator<NotificationUpdateRequestDto> updateValidator)
    {
        _repo = repo;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public Task<NotificationResponseDto> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
    public Task<IEnumerable<NotificationResponseDto>> GetAllAsync() => _repo.GetAllAsync();
    public Task<PagedResponseDto<NotificationResponseDto>> QueryAsync(NotificationQueryDto query) => _repo.QueryAsync(query);

    public async Task<NotificationResponseDto> CreateAsync(NotificationCreateRequestDto request)
    {
        ValidationResult result = await _createValidator.ValidateAsync(request);
        if (!result.IsValid) throw new ValidationException(result.Errors);
        return await _repo.CreateAsync(request);
    }

    public async Task<NotificationResponseDto> UpdateAsync(NotificationUpdateRequestDto request)
    {
        ValidationException result = await new NotificationUpdateRequestDtoValidator().ValidateAsync(request) is ValidationResult vr && !vr.IsValid ? throw new ValidationException(vr.Errors) : null;
        return await _repo.UpdateAsync(request);
    }

    public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    public Task BulkDeleteAsync(BulkDeleteNotificationsRequestDto request) => _repo.BulkDeleteAsync(request);
}