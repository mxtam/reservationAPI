namespace reservationAPI.Models
{
    public class Apartment
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Rooms { get; set; } = 1;
        public decimal PricePerDay { get; set; } = 500;
        public int MaxGuests { get; set; } = 2;
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public List<Booking>? Bookings { get; set; } 
    }
}
