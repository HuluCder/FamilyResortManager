namespace FamilyResortManager.Services.DTOs;

public class ShiftCreateRequestDto
{
    public int StaffId { get; set; }
    public DateTime ShiftDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}