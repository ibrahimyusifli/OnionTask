using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProniaOnion202.Application.Abstractions.Repositories;
using ProniaOnion202.Application.Abstractions.Services;
using ProniaOnion202.Domain.Entities;
using ProniaOnion202.Persistence.Contexts;
using ProniaOnion202.Persistence.Implementations.Repositories;
using ProniaOnion202.Persistence.Implementations.Services;
using System.Reflection;


namespace ProniaOnion202.Persistence.ServiceRegistiration
{
    public static class ServiceRegistiration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("Default"),
                b=>b.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)));
            services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 8;

                opt.User.RequireUniqueEmail = true;

                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromMinutes(5);
                opt.Lockout.AllowedForNewUsers = true;
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IColorRepository, ColorRepository>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();





            return services;
        }
    }
}
