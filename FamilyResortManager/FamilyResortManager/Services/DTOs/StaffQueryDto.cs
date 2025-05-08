namespace FamilyResortManager.Services.DTOs;

public class StaffQueryDto : PageRequestDto
{
    public string? Name { get; set; }
    public string? Position { get; set; }
    public string? Email { get; set; }
}