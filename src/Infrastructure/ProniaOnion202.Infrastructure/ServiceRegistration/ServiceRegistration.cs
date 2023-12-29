
using ProniaOnion202.Infrastructure.Implementations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion202.Infrastructure.ServiceRegistration
{
    internal static class ServiceRegistration
    {
        public static IServiceCollection AddInfraStructureServices(this ISerciceCollection services)
        {
            services.AddScoped<ITokenHandler, TokenHandler>();

            services.AddAuthentication();
            return services;
        }
    }
}
