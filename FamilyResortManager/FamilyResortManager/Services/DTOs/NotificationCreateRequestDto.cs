namespace FamilyResortManager.Services.DTOs;

public class NotificationCreateRequestDto
{
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}