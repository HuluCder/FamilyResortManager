using FamilyResortManager.Services.DTOs;

namespace FamilyResortManager.Data.DataBase.Interface;

public interface IAuditLogRepository
{
    Task<AuditLogResponseDto> GetByIdAsync(int id);
    Task<IEnumerable<AuditLogResponseDto>> GetAllAsync();
    Task<PagedResponseDto<AuditLogResponseDto>> QueryAsync(PageRequestDto query);
    Task<AuditLogResponseDto> CreateAsync(AuditLogCreateRequestDto request);
    Task DeleteAsync(int id);
    Task BulkDeleteAsync(BulkDeleteAuditLogsRequestDto request);
}