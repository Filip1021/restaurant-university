namespace RestaurantReservation.Host.Contracts.Requests.Slots;

public class ReservationSlotResponse
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime SlotTimeUtc { get; set; }
    public decimal Price { get; set; }
    public int AvailableSeats { get; set; }
    public bool IsActive { get; set; } = true;
}
