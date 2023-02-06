using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTB.CAVU.ParkingSpaces.Services.Parking;

namespace TTB.CAVU.ParkingSpaces.Services
{
    public static class DependencyRegistrar
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddTransient<IParkingService, ParkingService>();
            return services;
        }
    }
}
