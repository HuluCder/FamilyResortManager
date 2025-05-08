namespace FamilyResortManager.Services.DTOs
{
    /// <summary>
    /// Параметры фильтрации и пагинации для AuditLog
    /// </summary>
    public class AuditLogQueryDto : PageRequestDto
    {
        /// <summary>Фильтр по действию</summary>
        public string? Action { get; set; }
        /// <summary>Начальная дата создания лога</summary>
        public DateTime? CreatedAtFrom { get; set; }
        /// <summary>Конечная дата создания лога</summary>
        public DateTime? CreatedAtTo { get; set; }
    }
}