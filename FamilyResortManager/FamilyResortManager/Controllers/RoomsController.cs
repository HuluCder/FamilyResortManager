using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FamilyResortManager.Services;
using System.Collections.Generic;
using FamilyResortManager.Services.DTOs;
using FamilyResortManager.Services.Interfaces;

namespace FamilyResortManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _service;
        public RoomsController(IRoomService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<PagedResponseDto<RoomResponseDto>>> Get([FromQuery] RoomQueryDto query)
            => Ok(await _service.QueryAsync(query));

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomResponseDto>> Get(int id)
            => Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        public async Task<ActionResult<RoomResponseDto>> Create(RoomCreateRequestDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RoomResponseDto>> Update(int id, RoomUpdateRequestDto dto)
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
        public async Task<IActionResult> BulkDelete(BulkDeleteRoomsRequestDto dto)
        {
            await _service.BulkDeleteAsync(dto);
            return NoContent();
        }
    }
}
