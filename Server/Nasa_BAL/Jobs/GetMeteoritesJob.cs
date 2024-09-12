using NAS_BAL.Entities;
using Nasa_BAL.Interfaces;
using Quartz;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Quartz.Logging;

namespace Nasa_BAL.Jobs
{
    public class GetMeteoritesJob : IJob
    {
        private readonly IMeteoriteService _meteoriteService;

        private readonly HttpClient _httpClient;

        private readonly ILogger<GetMeteoritesJob> _logger;

        public GetMeteoritesJob(IMeteoriteService meteoriteService, HttpClient httpClient, ILogger<GetMeteoritesJob> logger)
        {
            _meteoriteService = meteoriteService ?? throw new ArgumentNullException(nameof(meteoriteService));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var response = await _httpClient.GetStringAsync("https://data.nasa.gov/resource/y77d-th95.json");
                var meteorites = JsonConvert.DeserializeObject<List<Meteorite>>(response);

                await _meteoriteService.DecisionMakingCenter(meteorites);

                _logger.LogInformation("Yuhoo!!! GetMeteoritesJob executed successfull");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Oops( Error executing GetMeteoritesJob.");
            }
        }
    }
}
