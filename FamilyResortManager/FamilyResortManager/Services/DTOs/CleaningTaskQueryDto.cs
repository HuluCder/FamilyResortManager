namespace FamilyResortManager.Services.DTOs;

public class CleaningTaskQueryDto : PageRequestDto
{
    public int? RoomId { get; set; }
    public int? StaffId { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public string? Status { get; set; }
}