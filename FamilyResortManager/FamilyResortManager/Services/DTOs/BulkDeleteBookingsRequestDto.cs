namespace FamilyResortManager.Services.DTOs;

public class BulkDeleteBookingsRequestDto
{
    public List<int> Ids { get; set; } = new List<int>();
}