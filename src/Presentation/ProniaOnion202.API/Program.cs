namespace ProniaOnion202.API
{
    using FluentValidation;
    using FluentValidation.AspNetCore;
    using ProniaOnion202.Application.DTOs.Categories;
    using ProniaOnion202.Application.ServiceRegistration;
    using ProniaOnion202.Application.Validators;
    using ProniaOnion202.Persistence.Contexts;
    using ProniaOnion202.Persistence.ServiceRegistiration;
    public class Program
    {
        public static void Main(string[] args)
        {
            
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
    
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddApplicationServices();
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddInfrastructureServices(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            using (var scope = app.Services.CreateScope())
            {
            var initializer = scope.ServiceProvider.GetRequiredService<AppDbContextInitializer>();
            initializer.InitializeDbContext();
            initializer.CreateRoleAsync().Wait();
            initializer.InitializeAdmin().Wait();
            }
               

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
