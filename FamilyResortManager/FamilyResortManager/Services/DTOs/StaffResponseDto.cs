namespace FamilyResortManager.Services.DTOs;

public class StaffResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public double HourlyRate { get; set; }
}