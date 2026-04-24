using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsPro.Models;

namespace SportsPro.DataLayer.Configuration
{
    internal class RegistrationConfig : IEntityTypeConfiguration<Registration>
    {
        public void Configure(EntityTypeBuilder<Registration> entity)
        {
            // Composite primary key
            entity.HasKey(r => new { r.CustomerID, r.ProductID });

            // Many-to-many relationship Customer -> Registrations -> Product
            entity.HasOne(r => r.Customer)
                  .WithMany(c => c.Registrations)
                  .HasForeignKey(r => r.CustomerID);

            entity.HasOne(r => r.Product)
                  .WithMany(p => p.Registrations)
                  .HasForeignKey(r => r.ProductID);
        }
    }
}
