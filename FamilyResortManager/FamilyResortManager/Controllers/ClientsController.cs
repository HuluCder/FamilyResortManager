using FamilyResortManager.Services.DTOs;
using FamilyResortManager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FamilyResortManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _service;
    public ClientsController(IClientService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<ClientResponseDto>>> Get([FromQuery] ClientQueryDto query)
        => Ok(await _service.QueryAsync(query));

    [HttpGet("{id}")]
    public async Task<ActionResult<ClientResponseDto>> Get(int id)
        => Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<ActionResult<ClientResponseDto>> Create(ClientCreateRequestDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ClientResponseDto>> Update(int id, ClientUpdateRequestDto dto)
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
    public async Task<IActionResult> BulkDelete(BulkDeleteClientsRequestDto dto)
    {
        await _service.BulkDeleteAsync(dto);
        return NoContent();
    }
}