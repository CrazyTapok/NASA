using NAS_BAL.Entities;
using Nasa_BAL.Models;

namespace Nasa_BAL.Interfaces
{
    public interface IMeteoriteService
    {
        Task CreateMeteoriteAsync(Meteorite meteorite);
        Task UpdateMeteoriteAsync(Meteorite oldmMeteorite, Meteorite newMeteorite);
        Task RemoveMeteoritesAsync(List<Meteorite> meteoritesToRemove);
        Task DecisionMakingCenter(List<Meteorite> newMeteorites);
        Task<List<MeteoriteGroup>> GetMeteoritesAsync(int? startYear, int? endYear, string? recclass, string? namePart, string? sortBy, bool ascending);
    }
}
