namespace FamilyResortManager.Services.DTOs;

public class ClientQueryDto : PageRequestDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
}