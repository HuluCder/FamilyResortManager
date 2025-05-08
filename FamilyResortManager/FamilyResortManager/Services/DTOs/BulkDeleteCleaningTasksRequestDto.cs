namespace FamilyResortManager.Services.DTOs;

public class BulkDeleteCleaningTasksRequestDto
{
    public List<int> Ids { get; set; } = new List<int>();
}