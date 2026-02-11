using RestaurantReservation.BL.Dtos;

namespace RestaurantReservation.BL.Interfaces;

public interface IReservationService
{
    Task<ReservationResultDto> ReserveAsync(string userId, string slotId, int quantity, CancellationToken ct);
}
