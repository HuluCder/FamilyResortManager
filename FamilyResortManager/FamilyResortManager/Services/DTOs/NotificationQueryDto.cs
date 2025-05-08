namespace FamilyResortManager.Services.DTOs;

/// <summary>
/// Параметры фильтрации и пагинации для Notifications
/// </summary>
public class NotificationQueryDto : PageRequestDto
{
    /// <summary>Фильтр по тексту сообщения</summary>
    public string? Message { get; set; }
    /// <summary>Начальная дата уведомления</summary>
    public DateTime? CreatedAtFrom { get; set; }
    /// <summary>Конечная дата уведомления</summary>
    public DateTime? CreatedAtTo { get; set; }
}