using Nasa_DAL.Enum;

namespace Nasa_DAL.Entities
{
    public class Geolocation
    {
        public int Id { get; set; }
        public ETypeGeolocation Type { get; set; }
        public List<double> Coordinates { get; set; }
    }
}
