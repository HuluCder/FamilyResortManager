namespace FamilyResortManager.Services.DTOs;

public class CleaningTaskUpdateRequestDto
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public int StaffId { get; set; }
    public DateTime TaskDate { get; set; }
    public string Status { get; set; } = string.Empty;
}