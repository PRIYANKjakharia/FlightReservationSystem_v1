using FlightReservationSystem_v1.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightReservationSystem_v1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Additional model configurations can be added here
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Ensure that you have your actual connection string here
                optionsBuilder.UseMySql("your_connection_string_here", new MySqlServerVersion(new Version(8, 0, 21)));
            }
        }
    }
}
