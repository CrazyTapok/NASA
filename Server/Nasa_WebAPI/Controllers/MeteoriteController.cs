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
        ///<param name = "recClass" > Meteorite class.</param>
        ///<param name = "namePart" > Part of the meteorite name.</param>
        ///<param name = "sortBy" > Sort field. ("year", "count", "totalmass")</param>
        ///<param name = "ascending" > Sort order. </param>
        ///<returns> List of meteorites.</returns>
        [HttpGet("GetMeteorites")]
        public async Task<IActionResult> GetMeteorites([FromQuery] int? startYear, [FromQuery] int? endYear, [FromQuery] string? recClass, [FromQuery] string? namePart, [FromQuery] string? sortBy, [FromQuery] bool ascending)
        {
            try
            {
                var key = $"{startYear}-{endYear}-{recClass}-{namePart}-{sortBy}-{ascending}";

                if (!_cache.TryGetValue(key, out var meteoriteGroups))
                {
                    meteoriteGroups = await _meteoriteService.GetMeteoritesAsync(startYear, endYear, recClass, namePart, sortBy, ascending);

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

        /// <summary>
        /// Get minimum and maximum year.
        /// </summary>
        ///<returns> Minimum and maximum year.</returns>
        [HttpGet("GetMinMaxYear")]
        public async Task<IActionResult> GetMinMaxYear()
        {
            try
            {
                var (minYear, maxYear) = await _meteoriteService.GetMinMaxYearAsync();

                return Ok(new { MinYear = minYear, MaxYear = maxYear });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "An unexpected error occurred. Please try again later." });
            }
        }

        /// <summary>
        /// Get unique RecClass values.
        /// </summary>
        ///<returns> List of unique RecClass values.</returns>
        [HttpGet("GetUniqueRecClass")]
        public async Task<IActionResult> GetUniqueRecClass()
        {
            try
            {
                var uniqueRecClasses = await _meteoriteService.GetUniqueRecClassAsync();

                return Ok(uniqueRecClasses);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "An unexpected error occurred. Please try again later." });
            }
        }
    }
}
