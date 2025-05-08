namespace FamilyResortManager.Services.DTOs;

public class BulkDeleteStaffRequestDto
{
    public List<int> Ids { get; set; } = new List<int>();
}