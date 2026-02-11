using RestaurantReservation.BL.Dtos;
using RestaurantReservation.BL.Exceptions;
using RestaurantReservation.BL.Interfaces;
using RestaurantReservation.DAL.Entities;
using RestaurantReservation.DAL.Interfaces;

namespace RestaurantReservation.BL.Services;

public class ReservationSlotsService : IReservationSlotService
{
    private readonly IReservationSlotRepository _slots;

    public ReservationSlotsService(IReservationSlotRepository slots)
    {
        _slots = slots;
    }

    public async Task<List<ReservationSlotDto>> GetAllAsync(CancellationToken ct = default)
    {
        var items = await _slots.GetAllAsync(ct);
        return items.Select(ToDto).ToList();
    }

    public async Task<ReservationSlotDto> GetByIdAsync(string id, CancellationToken ct = default)
    {
        var entity = await _slots.GetByIdAsync(id, ct);
        if (entity is null)
            throw new NotFoundException($"Reservation slot with id '{id}' was not found.");

        return ToDto(entity);
    }

    public async Task<ReservationSlotDto> CreateAsync(ReservationSlotDto dto, CancellationToken ct = default)
    {
        var entity = new ReservationSlotEntity
        {
            Name = dto.Name,
            Location = dto.Location,
            SlotTimeUtc = dto.SlotTimeUtc,
            Price = dto.Price,
            AvailableSeats = dto.AvailableSeats,
            IsActive = dto.IsActive
        };

        await _slots.CreateAsync(entity, ct);
        return ToDto(entity);
    }

    public async Task<ReservationSlotDto> UpdateAsync(string id, ReservationSlotDto dto, CancellationToken ct = default)
    {
        var existing = await _slots.GetByIdAsync(id, ct);
        if (existing is null)
            throw new NotFoundException($"Reservation slot with id '{id}' was not found.");

        existing.Name = dto.Name;
        existing.Location = dto.Location;
        existing.SlotTimeUtc = dto.SlotTimeUtc;
        existing.Price = dto.Price;
        existing.AvailableSeats = dto.AvailableSeats;
        existing.IsActive = dto.IsActive;

        await _slots.UpdateAsync(existing, ct);
        return ToDto(existing);
    }

    public async Task DeleteAsync(string id, CancellationToken ct = default)
    {
        var existing = await _slots.GetByIdAsync(id, ct);
        if (existing is null)
            throw new NotFoundException($"Reservation slot with id '{id}' was not found.");

        await _slots.DeleteAsync(id, ct);
    }

    private static ReservationSlotDto ToDto(ReservationSlotEntity e) => new()
    {
        Id = e.Id,
        Name = e.Name,
        Location = e.Location,
        SlotTimeUtc = e.SlotTimeUtc,
        Price = e.Price,
        AvailableSeats = e.AvailableSeats,
        IsActive = e.IsActive
    };
}
