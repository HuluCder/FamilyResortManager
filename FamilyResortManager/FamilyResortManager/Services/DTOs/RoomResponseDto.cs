namespace FamilyResortManager.Services.DTOs;

public class RoomResponseDto
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public double Price { get; set; }
    public string Status { get; set; } = string.Empty;
}