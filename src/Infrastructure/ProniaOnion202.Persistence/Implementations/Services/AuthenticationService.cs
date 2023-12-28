using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProniaOnion202.Application.DTOs.Users;
using ProniaOnion202.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion202.Persistence.Implementations.Services
{
    internal class AuthenticationService:IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public AuthenticationService(UserManager<AppUser> userManager,IMapper mapper)
        {
            _userManager=userManager;
            _mapper = mapper;
        }
        public async Task Login(LoginDto dto)
        {
            AppUser user = await _userManager.FindByNameAsync(dto.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(dto.UserNameOrEmail);
                if (user == null) throw new Exception("Username,Email or Password is incorrect");
            }
            if (!await _userManager.CheckPasswordAsync(user, dto.Password)) throw new Exception("Username,Email or Password is incorrect");

        }

        public async Task Register(RegisterDto dto)
        {
            if (await _userManager.Users.AnyAsync(u => u.UserName == dto.UserName || u.Email == dto.Email)) throw new Exception("Same");
            AppUser user=_mapper.Map<AppUser>(dto);
            var result = await _userManager.CreateAsync(user,dto.Password);

            if (!result.Succeeded)
            {
                StringBuilder builder = new StringBuilder();
                
                foreach (var error in result.Errors)
                {
                    builder.AppendLine(error.Description);
                }
                throw new Exception(builder.ToString());
            }  
        }
    }
}
