namespace RestaurantReservation.BL.Dtos;

public class ReservationResultDto
{
    public string SlotId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public int RemainingSeats { get; set; }
}
