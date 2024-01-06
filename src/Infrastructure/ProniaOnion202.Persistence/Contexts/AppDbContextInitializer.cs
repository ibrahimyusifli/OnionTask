using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProniaOnion202.Domain.Entities;
using ProniaOnion202.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion202.Persistence.Contexts
{
    public class AppDbContextInitializer
    {
        private readonly AppDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public AppDbContextInitializer(
            AppDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager,
            IConfiguration configuration
            )
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task InitializeDbContext()
        {
            await _context.Database.MigrateAsync();
        }

        public async Task CreateRoleAsync()
        {
            foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                if (await _roleManager.RoleExistsAsync(role.ToString()))
                    await _roleManager.CreateAsync(new IdentityRole { Name=role.ToString()});
            }
        }
        public async Task InitializeAdmin()
        {
            AppUser admin = new AppUser
            {
                Name = "Admin",
                Surname="Admin",
                Email = _configuration["AdminSettings:Email"],
                UserName= _configuration["AdminSettings:UserName"]
            };

            await _userManager.CreateAsync(admin, _configuration["AdminSettings:Password"]);
            await _userManager.AddToRoleAsync(admin, UserRole.Admin.ToString());
           
        }
    }
}
