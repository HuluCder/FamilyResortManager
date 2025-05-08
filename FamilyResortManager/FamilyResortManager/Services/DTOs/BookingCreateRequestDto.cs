namespace FamilyResortManager.Services.DTOs;

public class BookingCreateRequestDto
{
    public int RoomId { get; set; }
    public int ClientId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public string Status { get; set; } = string.Empty;
}