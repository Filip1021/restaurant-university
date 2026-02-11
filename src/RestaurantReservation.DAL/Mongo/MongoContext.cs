using RestaurantReservation.DAL.Entities;
using MongoDB.Driver;

namespace RestaurantReservation.DAL.Mongo;

public class MongoContext
{
    private readonly IMongoDatabase _db;

    public MongoContext(IMongoDatabase db)
    {
        _db = db;
    }

    public IMongoCollection<ReservationSlotEntity> ReservationSlots => _db.GetCollection<ReservationSlotEntity>("slots");
    public IMongoCollection<UserEntity> Users => _db.GetCollection<UserEntity>("users");
}
