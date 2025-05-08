using FamilyResortManager.Services.DTOs;

namespace FamilyResortManager.Services.Interfaces;

public interface IAuditLogService
{
    Task<AuditLogResponseDto> GetByIdAsync(int id);
    Task<IEnumerable<AuditLogResponseDto>> GetAllAsync();
    Task<PagedResponseDto<AuditLogResponseDto>> QueryAsync(AuditLogQueryDto query);
    Task<AuditLogResponseDto> CreateAsync(AuditLogCreateRequestDto request);
    Task DeleteAsync(int id);
    Task BulkDeleteAsync(BulkDeleteAuditLogsRequestDto request);
}