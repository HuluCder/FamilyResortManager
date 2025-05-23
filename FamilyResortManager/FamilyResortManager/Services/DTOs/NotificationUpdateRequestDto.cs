namespace FamilyResortManager.Services.DTOs;

public class NotificationUpdateRequestDto
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}