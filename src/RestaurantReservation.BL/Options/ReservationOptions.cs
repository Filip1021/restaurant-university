namespace RestaurantReservation.BL.Options;

public class ReservationOptions
{
    public int MaxSeatsPerUser { get; set; } = 5;
    public decimal ReservationFeePercent { get; set; } = 0m; // 0.05 = 5%
    public bool AllowReservationAfterStart { get; set; } = false;
}
