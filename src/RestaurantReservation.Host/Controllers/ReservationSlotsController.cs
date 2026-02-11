using RestaurantReservation.BL.Dtos;
using RestaurantReservation.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantReservation.Host.Controllers;

[ApiController]
[Route("api/slots")]
public class ReservationSlotsController : ControllerBase
{
    private readonly IReservationSlotService _slotsService;

    public ReservationSlotsController(IReservationSlotService slotsService)
    {
        _slotsService = slotsService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ReservationSlotDto>>> GetAll(CancellationToken ct)
    {
        var result = await _slotsService.GetAllAsync(ct);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReservationSlotDto>> GetById(string id, CancellationToken ct)
    {
        var result = await _slotsService.GetByIdAsync(id, ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ReservationSlotDto>> Create([FromBody] ReservationSlotDto dto, CancellationToken ct)
    {
        var created = await _slotsService.CreateAsync(dto, ct);
        return Ok(created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ReservationSlotDto>> Update(string id, [FromBody] ReservationSlotDto dto, CancellationToken ct)
    {
        var updated = await _slotsService.UpdateAsync(id, dto, ct);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        await _slotsService.DeleteAsync(id, ct);
        return NoContent();
    }
}
