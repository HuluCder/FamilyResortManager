namespace FamilyResortManager.Services.DTOs;

public class BulkDeleteShiftsRequestDto
{
    public List<int> Ids { get; set; } = new List<int>();
}