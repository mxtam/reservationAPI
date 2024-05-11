namespace reservationAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = new byte[0];
        public byte[] PasswordSalt { get; set; } = new byte[0];

        public int? ApartmentId { get; set; }
        public Apartment? Apartment { get; set; }

        public List<Booking>? Bookings { get; set; }
    }
}
