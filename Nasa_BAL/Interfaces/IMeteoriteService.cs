using NAS_BAL.Entities;
namespace Nasa_BAL.Interfaces
{
    public interface IMeteoriteService
    {
        Task CreateMeteoriteAsync(Meteorite meteorite);
        Task UpdateMeteoriteAsync(Meteorite oldmMeteorite, Meteorite newMeteorite);
        Task RemoveMeteoritesAsync(List<Meteorite> meteoritesToRemove);
        Task DecisionMakingCenter(List<Meteorite> newMeteorites);
    }
}
