namespace RestaurantReservation.Host.Dtos;

public class ReservationRequestDto
{
    public string UserId { get; set; } = string.Empty;
    public string SlotId { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
