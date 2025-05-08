namespace FamilyResortManager.Services.DTOs;

public class AuditLogCreateRequestDto
{
    public string Action { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}