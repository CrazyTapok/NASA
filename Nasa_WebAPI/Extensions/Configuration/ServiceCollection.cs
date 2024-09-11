using Nasa_BAL.Interfaces;
using Nasa_BAL.Services;

namespace Nasa_WebAPI.Extensions.Configuration
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IMeteoriteService, MeteoriteService>();

            return services;
        }
    }
}
