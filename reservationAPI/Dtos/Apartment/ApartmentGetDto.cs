namespace reservationAPI.Dtos.Apartment
{
    public class ApartmentGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Rooms { get; set; } = 1;
        public string? PricePerDay { get; set; }
        public int MaxGuests { get; set; } = 2;
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
