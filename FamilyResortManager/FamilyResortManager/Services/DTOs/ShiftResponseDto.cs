namespace FamilyResortManager.Services.DTOs;

public class ShiftResponseDto
{
    public int Id { get; set; }
    public int StaffId { get; set; }
    public DateTime ShiftDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}