namespace reservationAPI.Dtos.Booking
{
    //DTO для бронювання житла
    public class BookApartmentDto
    {
        public DateTime Entry { get; set; } 
        public DateTime Leave { get; set; }
        public int Guests { get; set; }
    }
}
