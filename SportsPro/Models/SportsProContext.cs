using Microsoft.EntityFrameworkCore;
using SportsPro.DataLayer.Configuration;

namespace SportsPro.Models
{
    public class SportsProContext : DbContext
    {
        public SportsProContext(DbContextOptions<SportsProContext> options)
            : base(options)
        { }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Technician> Technicians { get; set; } = null!;
        public DbSet<Country> Countries { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Incident> Incidents { get; set; } = null!;
        public DbSet<Registration> Registrations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfig());
            modelBuilder.ApplyConfiguration(new TechnicianConfig());
            modelBuilder.ApplyConfiguration(new CountryConfig());
            modelBuilder.ApplyConfiguration(new CustomerConfig());
            modelBuilder.ApplyConfiguration(new IncidentConfig());
            modelBuilder.ApplyConfiguration(new RegistrationConfig());
        }
    }
}
