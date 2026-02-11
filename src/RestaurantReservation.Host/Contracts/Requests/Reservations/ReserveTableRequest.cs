namespace RestaurantReservation.Host.Contracts.Requests.Reservations;

public class ReserveTableRequest
{
    public string UserId { get; set; } = string.Empty;
    public string SlotId { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
