using FamilyResortManager.Services.DTOs;
using FamilyResortManager.Services.Implementation;
using FamilyResortManager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FamilyResortManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StaffController : ControllerBase
{
    private readonly IStaffService _service;
    public StaffController(IStaffService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<StaffResponseDto>>> Get([FromQuery] StaffQueryDto query)
        => Ok(await _service.QueryAsync(query));

    [HttpGet("{id}")]
    public async Task<ActionResult<StaffResponseDto>> Get(int id)
        => Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<ActionResult<StaffResponseDto>> Create(StaffCreateRequestDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<StaffResponseDto>> Update(int id, StaffUpdateRequestDto dto)
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
    public async Task<IActionResult> BulkDelete(BulkDeleteStaffRequestDto dto)
    {
        await _service.BulkDeleteAsync(dto);
        return NoContent();
    }
}