using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using reservationAPI.Models;

namespace reservationAPI.Data
{
    public class DataContext:DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Booking> Booking { get; set; }


        public DataContext(DbContextOptions<DataContext> options):base(options) 
        {

        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Apartment)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.ApartmentId);

            modelBuilder.Entity<Apartment>().HasData(
                new Apartment { Id = 1, Name = "King`s Luxe", Rooms = 5, Description = "King`s Luxe with 5 rooms", 
                    Location = "м.Київ", MaxGuests = 4, PricePerDay = 80000.00m },
                new Apartment { Id = 2, Name = "Luxe", Rooms = 4, Description = "Luxe with 4 rooms", 
                    Location = "м.Київ", MaxGuests = 4, PricePerDay = 60000.00m });
        }
        
    }
}
