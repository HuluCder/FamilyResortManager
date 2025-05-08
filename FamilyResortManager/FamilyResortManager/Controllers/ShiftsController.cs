using FamilyResortManager.Services.DTOs;
using FamilyResortManager.Services.Implementation;
using FamilyResortManager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FamilyResortManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShiftsController : ControllerBase
{
    private readonly IShiftService _service;
    public ShiftsController(IShiftService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<ShiftResponseDto>>> Get([FromQuery] ShiftQueryDto query)
        => Ok(await _service.QueryAsync(query));

    [HttpGet("{id}")]
    public async Task<ActionResult<ShiftResponseDto>> Get(int id)
        => Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<ActionResult<ShiftResponseDto>> Create(ShiftCreateRequestDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ShiftResponseDto>> Update(int id, ShiftUpdateRequestDto dto)
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
    public async Task<IActionResult> BulkDelete(BulkDeleteShiftsRequestDto dto)
    {
        await _service.BulkDeleteAsync(dto);
        return NoContent();
    }
}