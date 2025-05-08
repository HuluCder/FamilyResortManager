using FamilyResortManager.Services.DTOs;
using FamilyResortManager.Services.Implementation;
using FamilyResortManager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FamilyResortManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditLogsController : ControllerBase
{
    private readonly IAuditLogService _service;
    public AuditLogsController(IAuditLogService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<AuditLogResponseDto>>> Get([FromQuery] AuditLogQueryDto query)
        => Ok(await _service.QueryAsync(query));

    [HttpGet("{id}")]
    public async Task<ActionResult<AuditLogResponseDto>> Get(int id)
        => Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<ActionResult<AuditLogResponseDto>> Create(AuditLogCreateRequestDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("bulk-delete")]
    public async Task<IActionResult> BulkDelete(BulkDeleteAuditLogsRequestDto dto)
    {
        await _service.BulkDeleteAsync(dto);
        return NoContent();
    }
}