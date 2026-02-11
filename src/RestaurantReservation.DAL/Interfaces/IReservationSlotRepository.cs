using RestaurantReservation.DAL.Entities;

namespace RestaurantReservation.DAL.Interfaces;

public interface IReservationSlotRepository
{
    Task<List<ReservationSlotEntity>> GetAllAsync(CancellationToken ct);
    Task<ReservationSlotEntity?> GetByIdAsync(string id, CancellationToken ct);
    Task<ReservationSlotEntity> CreateAsync(ReservationSlotEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(ReservationSlotEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(string id, CancellationToken ct);
}
