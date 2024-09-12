using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Nasa_BAL.Interfaces;
using System.Net;

namespace Nasa_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeteoriteController : ControllerBase
    {
        private readonly IMeteoriteService _meteoriteService;

        private readonly IMemoryCache _cache;

        public MeteoriteController(IMeteoriteService meteoriteService, IMemoryCache cache)
        {
            _meteoriteService = meteoriteService;
            _cache = cache;
        }

        /// <summary>
        /// Get sorted, filtered, and grouped meteorites.
        /// </summary>
        ///<param name="startYear">Start year.</param>
        ///<param name = "endYear" > End year.</param>
        ///<param name = "recclass" > Meteorite class.</param>
        ///<param name = "namePart" > Part of the meteorite name.</param>
        ///<param name = "sortBy" > Sort field.</param>
        ///<param name = "ascending" > Sort order. ("year", "count", "totalmass") </param>
        ///<returns> List of meteorites.</returns>
        [HttpGet("GetMeteorites")]
        public async Task<IActionResult> GetMeteorites([FromQuery] int? startYear, [FromQuery] int? endYear, [FromQuery] string? recclass, [FromQuery] string? namePart, [FromQuery] string? sortBy, [FromQuery] bool ascending)
        {
            try
            {
                var key = $"{startYear}-{endYear}-{recclass}-{namePart}-{sortBy}-{ascending}";

                if (!_cache.TryGetValue(key, out var meteoriteGroups))
                {
                    meteoriteGroups = await _meteoriteService.GetMeteoritesAsync(startYear, endYear, recclass, namePart, sortBy, ascending);

                    var options = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(30));

                    _cache.Set(key, meteoriteGroups, options);
                }
           
                return Ok(meteoriteGroups);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "An unexpected error occurred. Please try again later." });
            }
        }
    }
}
