using RestaurantReservation.BL.Dtos;

namespace RestaurantReservation.BL.Interfaces;

public interface IReservationSlotService
{
    Task<List<ReservationSlotDto>> GetAllAsync(CancellationToken ct);
    Task<ReservationSlotDto> GetByIdAsync(string id, CancellationToken ct);
    Task<ReservationSlotDto> CreateAsync(ReservationSlotDto dto, CancellationToken ct);
    Task<ReservationSlotDto> UpdateAsync(string id, ReservationSlotDto dto, CancellationToken ct);
    Task DeleteAsync(string id, CancellationToken ct);
}
