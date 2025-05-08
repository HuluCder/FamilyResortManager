using FamilyResortManager.Services.DTOs;
using FamilyResortManager.Services.Implementation;
using FamilyResortManager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FamilyResortManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CleaningTasksController : ControllerBase
{
    private readonly ICleaningTaskService _service;
    public CleaningTasksController(ICleaningTaskService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<CleaningTaskResponseDto>>> Get([FromQuery] CleaningTaskQueryDto query)
        => Ok(await _service.QueryAsync(query));

    [HttpGet("{id}")]
    public async Task<ActionResult<CleaningTaskResponseDto>> Get(int id)
        => Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<ActionResult<CleaningTaskResponseDto>> Create(CleaningTaskCreateRequestDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CleaningTaskResponseDto>> Update(int id, CleaningTaskUpdateRequestDto dto)
    {
        if (id != dto.Id) return BadRequest();
        return Ok(await _service.UpdateAsync(dto));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("bulk-delete")]
    public async Task<IActionResult> BulkDelete(BulkDeleteCleaningTasksRequestDto dto)
    {
        await _service.BulkDeleteAsync(dto);
        return NoContent();
    }
}