using RestaurantReservation.DAL.Entities;
using RestaurantReservation.DAL.Mongo;
using MongoDB.Driver;

namespace RestaurantReservation.DAL.Repositories;

public class ReservationSlotRepository : Interfaces.IReservationSlotRepository
{
    private readonly MongoContext _ctx;

    public ReservationSlotRepository(MongoContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<List<ReservationSlotEntity>> GetAllAsync(CancellationToken ct)
        => await _ctx.ReservationSlots.Find(_ => true).ToListAsync(ct);

    public async Task<ReservationSlotEntity?> GetByIdAsync(string id, CancellationToken ct)
        => await _ctx.ReservationSlots.Find(x => x.Id == id).FirstOrDefaultAsync(ct);

    public async Task<ReservationSlotEntity> CreateAsync(ReservationSlotEntity entity, CancellationToken ct)
    {
        await _ctx.ReservationSlots.InsertOneAsync(entity, cancellationToken: ct);
        return entity;
    }

    public async Task<bool> UpdateAsync(ReservationSlotEntity entity, CancellationToken ct)
    {
        var result = await _ctx.ReservationSlots.ReplaceOneAsync(x => x.Id == entity.Id, entity, cancellationToken: ct);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken ct)
    {
        var result = await _ctx.ReservationSlots.DeleteOneAsync(x => x.Id == id, ct);
        return result.DeletedCount > 0;
    }
}
