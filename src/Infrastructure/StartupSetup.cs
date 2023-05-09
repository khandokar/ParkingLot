using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data.AdoRepositories;
using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlClient;

namespace Infrastructure
{
    public static class StartupSetup
    {
        public static void RegisterInfrastructureService(this IServiceCollection services, string? connectionString)
        {

            services.AddScoped<IDbManager, DbManager>(dm => new DbManager(new SqlConnection(connectionString)));

            services.AddScoped<IRepository<ParkIn>, ParkInRepository>();

            services.AddScoped<IRepository<ParkOut>, ParkOutRepository>();

            services.AddScoped<ICommonRepository, CommonRepository>();
        }
    }
   
}
