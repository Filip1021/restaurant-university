using RestaurantReservation.DAL.Entities;

namespace RestaurantReservation.DAL.Interfaces;

public interface IUserRepository
{
    Task<List<UserEntity>> GetAllAsync(CancellationToken ct);
    Task<UserEntity?> GetByIdAsync(string id, CancellationToken ct);
    Task<UserEntity> CreateAsync(UserEntity entity, CancellationToken ct);
    Task<bool> UpdateAsync(UserEntity entity, CancellationToken ct);
    Task<bool> DeleteAsync(string id, CancellationToken ct);
}
