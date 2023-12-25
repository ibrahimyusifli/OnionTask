using Microsoft.EntityFrameworkCore;
using ProniaOnion202.Domain.Entities;
using ProniaOnion202.Persistence.Common;
using ProniaOnion202.Persistence.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion202.Persistence.Contexts
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) 
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyQueryFilters();
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
           var entities = ChangeTracker.Entries<BaseEntity>();

            foreach (var data in entities)
            {
                switch (data.State)
                {
                    case EntityState.Modified:
                        data.Entity.ModifiedAd=DateTime.Now;    
                        break;
                    case EntityState.Added:
                        data.Entity.CreatedAt = DateTime.Now;

                        break;
                   
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
