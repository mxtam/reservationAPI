namespace reservationAPI.Dtos.Booking
{
    //DTO для редагування броні
    public class BookingEditDto
    {
        public DateTime Entry { get; set; }
        public DateTime Leave { get; set; }
        public int Guests { get; set; }
    }
}
