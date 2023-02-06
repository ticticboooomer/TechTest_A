using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTB.CAVU.ParkingSpaces.DataAccess.Config;
using TTB.CAVU.ParkingSpaces.DataAccess.Repository;
using TTB.CAVU.ParkingSpaces.DataAccess.Seeds;

namespace TTB.CAVU.ParkingSpaces.DataAccess
{
    public static class DependencyRegistrar
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<ConnectionSettings>(config.GetSection("Mongo"));
            services.AddSingleton<IAppRepository, AppRepository>();
            services.AddSingleton<IDatabaseSeedManager, DatabaseSeedManager>();
            return services;
        }
    }
}
