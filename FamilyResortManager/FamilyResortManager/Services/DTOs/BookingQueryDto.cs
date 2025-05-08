namespace FamilyResortManager.Services.DTOs;

public class BookingQueryDto : PageRequestDto
{
    public int? RoomId { get; set; }
    public int? ClientId { get; set; }
    public DateTime? CheckInDateFrom { get; set; }
    public DateTime? CheckInDateTo { get; set; }
    public string? Status { get; set; }
}