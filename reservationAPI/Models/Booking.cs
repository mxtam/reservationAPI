namespace reservationAPI.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime Entry { get; set; }
        public DateTime Leave { get; set; }
        public int Guests { get; set; }
        public decimal TotalPrice { get; set; }

        public int? ApartmentId { get; set; }
        public Apartment? Apartment { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
