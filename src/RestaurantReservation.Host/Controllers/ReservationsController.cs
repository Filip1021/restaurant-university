using RestaurantReservation.BL.Dtos;
using RestaurantReservation.BL.Interfaces;
using RestaurantReservation.Host.Dtos;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace RestaurantReservation.Host.Controllers;

[ApiController]
[Route("api/reservations")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly IValidator<ReservationRequestDto> _validator;

    public ReservationsController(IReservationService reservationService, IValidator<ReservationRequestDto> validator)
    {
        _reservationService = reservationService;
        _validator = validator;
    }

    [HttpPost("reserve")]
    public async Task<ActionResult<ReservationResultDto>> Reserve([FromBody] ReservationRequestDto dto, CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(dto, ct);
        if (!validation.IsValid)
            return BadRequest(validation.Errors);

        var result = await _reservationService.ReserveAsync(dto.UserId, dto.SlotId, dto.Quantity, ct);
        return Ok(result);
    }
}
