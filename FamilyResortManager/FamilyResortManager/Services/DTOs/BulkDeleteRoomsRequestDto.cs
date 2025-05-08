namespace FamilyResortManager.Services.DTOs;

public class BulkDeleteRoomsRequestDto
{
    public List<int> Ids { get; set; } = new List<int>();
}