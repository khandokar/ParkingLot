using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationCore
{
    public static class Dependencies
    {
        public static void RegisterCoreService(this IServiceCollection services)
        {
            services.AddScoped<IParkingService, ParkingService>();
        }
    }
}
