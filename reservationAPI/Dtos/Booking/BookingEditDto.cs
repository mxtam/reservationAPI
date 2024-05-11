namespace reservationAPI.Dtos.Booking
{
    public class BookingEditDto
    {
        public DateTime Entry { get; set; }
        public DateTime Leave { get; set; }
        public int Guests { get; set; }
    }
}
