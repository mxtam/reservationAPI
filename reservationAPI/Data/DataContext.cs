using Microsoft.EntityFrameworkCore;
using reservationAPI.Models;

namespace reservationAPI.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options) 
        {
        
        } 
        
        public DbSet<User> Users { get; set; }
    }
}
