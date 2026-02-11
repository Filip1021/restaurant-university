using RestaurantReservation.DAL.Entities;
using RestaurantReservation.DAL.Mongo;
using MongoDB.Driver;

namespace RestaurantReservation.DAL.Repositories;

public class UserRepository : Interfaces.IUserRepository
{
    private readonly MongoContext _ctx;

    public UserRepository(MongoContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<List<UserEntity>> GetAllAsync(CancellationToken ct)
        => await _ctx.Users.Find(_ => true).ToListAsync(ct);

    public async Task<UserEntity?> GetByIdAsync(string id, CancellationToken ct)
        => await _ctx.Users.Find(x => x.Id == id).FirstOrDefaultAsync(ct);

    public async Task<UserEntity> CreateAsync(UserEntity entity, CancellationToken ct)
    {
        await _ctx.Users.InsertOneAsync(entity, cancellationToken: ct);
        return entity;
    }

    public async Task<bool> UpdateAsync(UserEntity entity, CancellationToken ct)
    {
        var result = await _ctx.Users.ReplaceOneAsync(x => x.Id == entity.Id, entity, cancellationToken: ct);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken ct)
    {
        var result = await _ctx.Users.DeleteOneAsync(x => x.Id == id, ct);
        return result.DeletedCount > 0;
    }
}
