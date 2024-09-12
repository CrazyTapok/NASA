using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NAS_BAL.Entities;
using Nasa_DAL.Entities;

namespace NAS_BAL.EF
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Meteorite> Meteorites { get; set; } = null!;
        public DbSet<Geolocation> Geolocations { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
                : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(ApplicationContext)));

            base.OnModelCreating(modelBuilder);
        }
    }
}
