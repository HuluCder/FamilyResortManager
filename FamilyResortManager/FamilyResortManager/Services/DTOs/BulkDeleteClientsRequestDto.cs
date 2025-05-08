namespace FamilyResortManager.Services.DTOs;

public class BulkDeleteClientsRequestDto
{
    public List<int> Ids { get; set; } = new List<int>();
}