using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NAS_BAL.EF;
using NAS_BAL.Entities;
using Nasa_BAL.Interfaces;
using Nasa_BAL.Models;

namespace Nasa_BAL.Services
{
    public class MeteoriteService : IMeteoriteService
    {
        private readonly ApplicationContext _context;

        private readonly ILogger<MeteoriteService> _logger;

        public MeteoriteService(ApplicationContext context, ILogger<MeteoriteService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task CreateMeteoriteAsync(Meteorite meteorite)
        {
            try
            {
                _context.Meteorites.Add(meteorite);

                await _context.SaveChangesAsync();

                _logger.LogInformation("Yuhoo!!! Meteorite created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Oops( Error creating meteorite with ID:{meteorite.Id}");
                throw;
            }
        }

        public async Task RemoveMeteoritesAsync(List<Meteorite> meteoritesToRemove)
        {

            try
            {
                _context.Meteorites.RemoveRange(meteoritesToRemove);

                await _context.SaveChangesAsync();

                _logger.LogInformation("Yuhoo!!! Meteorites removed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Oops( Error removing meteorites");
                throw;
            }
        }

        public async Task UpdateMeteoriteAsync(Meteorite oldMeteorite, Meteorite newMeteorite)
        {
            try
            {
                oldMeteorite.Name = newMeteorite.Name;
                oldMeteorite.NameType = newMeteorite.NameType;
                oldMeteorite.RecClass = newMeteorite.RecClass;
                oldMeteorite.Mass = newMeteorite.Mass;
                oldMeteorite.Fall = newMeteorite.Fall;
                oldMeteorite.Year = newMeteorite.Year;
                oldMeteorite.Reclat = newMeteorite.Reclat;
                oldMeteorite.Reclong = newMeteorite.Reclong;
                oldMeteorite.Geolocation = newMeteorite.Geolocation;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Yuhoo!!! Meteorite updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Oops( Error updating meteorite with ID:{oldMeteorite?.Id}");
                throw;
            }

        }

        public async Task DecisionMakingCenter(List<Meteorite> newMeteorites)
        {
            try
            {
                var queryMeteorites = _context.Meteorites.Include(t => t.Geolocation).AsQueryable();
                var listMeteorites = await queryMeteorites.ToListAsync();
                var dictMeteorites = listMeteorites.ToDictionary(t => t.Id);

                var newMeteoritesDict = newMeteorites.ToDictionary(t => t.Id);

                var meteoritesToRemove = listMeteorites.Where(t => !newMeteoritesDict.ContainsKey(t.Id)).ToList();

                if (meteoritesToRemove.Count > 0)
                {
                    await RemoveMeteoritesAsync(meteoritesToRemove);
                }

                foreach (var newMeteorite in newMeteorites)
                {
                    if (dictMeteorites.TryGetValue(newMeteorite.Id, out var oldMeteorite))
                    {
                        await UpdateMeteoriteAsync(oldMeteorite, newMeteorite);
                    }
                    else
                    {
                        await CreateMeteoriteAsync(newMeteorite);
                    }
                }

                _logger.LogInformation("Yuhoo!!! DecisionMakingCenter completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Oops( Error executing GetMeteoritesJob");
            }
        }

        public async Task<List<MeteoriteGroup>> GetMeteoritesAsync(int? startYear, int? endYear, string? recclass, string? namePart, string? sortBy, bool ascending)
        {
            try
            {
                var query = _context.Meteorites.Include(m => m.Geolocation).AsQueryable();

                if (startYear.HasValue)
                {
                    query = query.Where(m => m.Year.HasValue && m.Year.Value.Year >= startYear.Value);
                }
                if (endYear.HasValue)
                {
                    query = query.Where(m => m.Year.HasValue && m.Year.Value.Year <= endYear.Value);
                }


                if (!string.IsNullOrEmpty(recclass))
                {
                    query = query.Where(m => m.RecClass == recclass);
                }


                if (!string.IsNullOrEmpty(namePart))
                {
                    query = query.Where(m => m.Name.Contains(namePart));
                }

                var groupedQuery = query.GroupBy(m => m.Year.HasValue ? m.Year.Value.Year : 0)
                                        .Select(g => new MeteoriteGroup
                                        {
                                            Year = g.Key,
                                            Count = g.Count(),
                                            TotalMass = g.Sum(m => m.Mass),
                                        });


                switch (sortBy?.ToLower())
                {
                    case "year":
                        groupedQuery = ascending ? groupedQuery.OrderBy(g => g.Year) : groupedQuery.OrderByDescending(g => g.Year);
                        break;
                    case "count":
                        groupedQuery = ascending ? groupedQuery.OrderBy(g => g.Count) : groupedQuery.OrderByDescending(g => g.Count);
                        break;
                    case "totalmass":
                        groupedQuery = ascending ? groupedQuery.OrderBy(g => g.TotalMass) : groupedQuery.OrderByDescending(g => g.TotalMass);
                        break;
                }

                return await groupedQuery.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Oops( Error get data.");
                
                throw;
            }
        }
    }
}
