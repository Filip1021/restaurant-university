using RestaurantReservation.BL.Dtos;
using RestaurantReservation.BL.Exceptions;
using RestaurantReservation.BL.Interfaces;
using RestaurantReservation.DAL.Interfaces;
using RestaurantReservation.BL.Options;
using Microsoft.Extensions.Options;

namespace RestaurantReservation.BL.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationSlotRepository _slots;
    private readonly IUserRepository _users;
    private readonly IOptionsMonitor<ReservationOptions> _options;

    public ReservationService(
        IReservationSlotRepository slots,
        IUserRepository users,
        IOptionsMonitor<ReservationOptions> options)
    {
        _slots = slots;
        _users = users;
        _options = options;
    }

    public async Task<ReservationResultDto> ReserveAsync(string userId, string slotId, int quantity, CancellationToken ct = default)
    {
        if (quantity <= 0)
            throw new BusinessRuleException("Quantity must be greater than 0.");

        var user = await _users.GetByIdAsync(userId, ct);
        if (user is null)
            throw new NotFoundException($"User with id '{userId}' was not found.");

        var slot = await _slots.GetByIdAsync(slotId, ct);
        if (slot is null)
            throw new NotFoundException($"Reservation slot with id '{slotId}' was not found.");

        var opts = _options.CurrentValue;

        if (!opts.AllowReservationAfterStart && slot.SlotTimeUtc <= DateTime.UtcNow)
            throw new BusinessRuleException("Reservation slot already started; reservation not allowed.");

        if (!slot.IsActive)
            throw new BusinessRuleException("Reservation slot is not active.");

        if (slot.AvailableSeats < quantity)
            throw new BusinessRuleException("Not enough seats available.");

        if (opts.MaxSeatsPerUser > 0 && user.ReservationsMade + quantity > opts.MaxSeatsPerUser)
            throw new BusinessRuleException("Reservation exceeds per-user seat limit.");

        slot.AvailableSeats -= quantity;
        await _slots.UpdateAsync(slot, ct);

        user.ReservationsMade += quantity;
        await _users.UpdateAsync(user, ct);

        var subtotal = slot.Price * quantity;
        var fee = subtotal * opts.ReservationFeePercent;
        var total = subtotal + fee;

        return new ReservationResultDto
        {
            SlotId = slotId,
            UserId = userId,
            Quantity = quantity,
            TotalPrice = total,
            RemainingSeats = slot.AvailableSeats
        };
    }
}
