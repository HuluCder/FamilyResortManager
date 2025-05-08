using FamilyResortManager.Services.DTOs;

namespace FamilyResortManager.Services.Interfaces;

public interface INotificationService
{
    Task<NotificationResponseDto> GetByIdAsync(int id);
    Task<IEnumerable<NotificationResponseDto>> GetAllAsync();
    Task<PagedResponseDto<NotificationResponseDto>> QueryAsync(NotificationQueryDto query);
    Task<NotificationResponseDto> CreateAsync(NotificationCreateRequestDto request);
    Task<NotificationResponseDto> UpdateAsync(NotificationUpdateRequestDto request);
    Task DeleteAsync(int id);
    Task BulkDeleteAsync(BulkDeleteNotificationsRequestDto request);
}