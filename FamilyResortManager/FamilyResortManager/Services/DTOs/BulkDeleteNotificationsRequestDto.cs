namespace FamilyResortManager.Services.DTOs;

public class BulkDeleteNotificationsRequestDto
{
    public List<int> Ids { get; set; } = new();
}