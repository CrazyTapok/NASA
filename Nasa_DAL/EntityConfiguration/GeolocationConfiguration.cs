using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Nasa_DAL.Entities;

namespace Nasa_DAL.EntityConfiguration
{
    public class GeolocationConfiguration : IEntityTypeConfiguration<Geolocation>
    {
        public void Configure(EntityTypeBuilder<Geolocation> builder)
        {

            builder.ToTable("Geolocations")
               .HasKey(t => t.Id);

            builder.Property(t => t.Type).IsRequired();

            builder.HasIndex(t => t.Type)
                   .HasDatabaseName("IX_Geolocation_Type");
        }
    }
}
