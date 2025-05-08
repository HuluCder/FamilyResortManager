namespace FamilyResortManager.Services.DTOs;

public class RoomQueryDto : PageRequestDto
{
    public string? Number { get; set; }
    public string? Type { get; set; }
    public double? MinPrice { get; set; }
    public double? MaxPrice { get; set; }
    public string? Status { get; set; }
}