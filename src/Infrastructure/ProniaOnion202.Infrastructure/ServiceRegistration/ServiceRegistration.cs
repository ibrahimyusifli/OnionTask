
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using a=ProniaOnion202.Infrastructure.Implementations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ProniaOnion202.Infrastructure.ServiceRegistration
{
    internal static class ServiceRegistration
    {
        public static IServiceCollection AddInfraStructureServices(this ISerciceCollection services,IConfiguration configuration)
        {
            services.AddScoped<ITokenHandler, a.TokenHandler>();

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidAudience= configuration["Jwt:Auidence"],
                    ValidIssuer = configuration["Jwt:Issuer"],
                    IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(["Jwt:SecurityKey"])),
                    LifetimeValidator = (nbf, expired, key, parameters) => key!=null?expired>DateTime.UtcNow:false
                    //LifetimeValidator = (nbf, expired, key, parameters) =>
                    //{
                    //    if (key is not null)
                    //    {
                    //        if (expired>DateTime.UtcNow)
                    //        {
                    //            return true;
                    //        }
                    //    }
                    //    return false;
                    //}
                };
            });

            services.AddAuthorization();
            return services;
        }
    }
}
