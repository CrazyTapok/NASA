using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NAS_BAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace NAS_BAL.EntityConfiguration
{
    public class MeteoriteConfiguration : IEntityTypeConfiguration<Meteorite>
    {
        public void Configure(EntityTypeBuilder<Meteorite> builder)
        {

            builder.ToTable("Meteorites")
               .HasKey(t => t.Id);

            builder.Property(t => t.Id).IsRequired();

            builder.Property(t => t.Name).IsRequired();

            builder.HasIndex(t => t.Name)
                   .HasDatabaseName("IX_Meteorite_Name");

            builder.HasIndex(t => t.RecClass)
                   .HasDatabaseName("IX_Meteorite_RecClass");

            builder.HasIndex(t => t.Year)
                   .HasDatabaseName("IX_Meteorite_Year");

            builder.HasOne(t => t.Geolocation)
                .WithMany()
                .HasForeignKey(t => t.GeolocationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
