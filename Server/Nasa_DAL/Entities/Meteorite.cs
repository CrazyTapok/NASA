using Nasa_DAL.Entities;
using Nasa_DAL.Enum;

namespace NAS_BAL.Entities
{
    public class Meteorite
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ENameType NameType { get; set; }
        public string RecClass { get; set; }
        public double Mass { get; set; }
        public string Fall { get; set; }
        public DateTime? Year { get; set; }
        public double Reclat { get; set; }
        public double Reclong { get; set; }
        public Geolocation Geolocation { get; set; }
    }
}
