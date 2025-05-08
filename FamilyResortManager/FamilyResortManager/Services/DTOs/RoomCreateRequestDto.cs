namespace FamilyResortManager.Services.DTOs;

public class RoomCreateRequestDto
{
    public string Number { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public double Price { get; set; }
    public string Status { get; set; } = string.Empty;
}
