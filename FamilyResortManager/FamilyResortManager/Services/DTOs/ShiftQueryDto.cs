namespace FamilyResortManager.Services.DTOs;

public class ShiftQueryDto : PageRequestDto
{
    public int? StaffId { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
}