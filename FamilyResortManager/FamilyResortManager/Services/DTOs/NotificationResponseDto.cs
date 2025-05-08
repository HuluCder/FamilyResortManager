namespace FamilyResortManager.Services.DTOs
{
    // ===========================
    // Notification DTOs
    // ===========================
    public class NotificationResponseDto
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }


    // ===========================
    // AuditLog DTOs
    // ===========================
}