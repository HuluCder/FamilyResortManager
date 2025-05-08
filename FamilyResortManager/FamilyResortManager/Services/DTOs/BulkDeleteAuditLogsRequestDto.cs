namespace FamilyResortManager.Services.DTOs;

public class BulkDeleteAuditLogsRequestDto
{
    public List<int> Ids { get; set; } = new();
}