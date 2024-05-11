namespace reservationAPI.Dtos.Booking
{
    //DTO для виводу інформації про бронь
    public class BookInfoDto
    {
        public int Id { get; set; }
        public string ApartmentName { get; set; }
        public string ApartmentLocation { get; set; }
        public DateTime Entry { get; set; }
        public DateTime Leave { get; set; }
        public int Guests { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
