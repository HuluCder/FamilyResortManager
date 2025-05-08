namespace FamilyResortManager.Services.DTOs;

public class ClientCreateRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Info { get; set; } = string.Empty;
}