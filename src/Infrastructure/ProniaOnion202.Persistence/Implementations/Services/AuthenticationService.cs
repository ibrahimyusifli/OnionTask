using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProniaOnion202.Application.Abstractions.Services;
using ProniaOnion202.Application.DTOs.Tokens;
using ProniaOnion202.Application.DTOs.Users;
using ProniaOnion202.Domain.Entities;
using ProniaOnion202.Domain.Enums;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion202.Persistence.Implementations.Services
{
    internal class AuthenticationService:IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenHandler _handler;

        public AuthenticationService(UserManager<AppUser> userManager,IMapper mapper,ITokenHandler handler)
        {
            _userManager=userManager;
            _mapper = mapper;
            _handler = handler;
        }

        public async Task Register(RegisterDto dto)
        {
            AppUser user=await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == dto.UserName || u.Email == dto.Email)) throw new Exception("Same");
            if (user == null) throw new Exception("same");
            user=_mapper.Map<AppUser>(dto);

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
            await _userManager.AddToRoleAsync(user, UserRole.Member.ToString());
        }

        public async Task<TokenResponseDto> Login(LoginDto dto)
        {
            AppUser user = await _userManager.FindByNameAsync(dto.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(dto.UserNameOrEmail);
                if (user == null) throw new Exception("Username,Email or Password is incorrect");
            }
            if (!await _userManager.CheckPasswordAsync(user, dto.Password)) throw new Exception("Username,Email or Password is incorrect");

            ICollection<Claim> claims = await _createClaims(user);
            TokenResponseDto tokendto = _handler.CreateJwt(user, claims, 60);
            user.RefreshToken = tokendto.RefreshToken;
            user.RefreshTokenExpiredAt = tokendto.RefreshExpireTime;
            await _userManager.UpdateAsync(user);
            return tokendto;

        }

        private async Task<ICollection<Claim>> _createClaims(AppUser user)
        {
            ICollection<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.GivenName,user.Name),
                new Claim(ClaimTypes.Surname,user.Surname)
            };
            foreach (var role in await _userManager.GetRolesAsync(user))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        public async Task<TokenResponseDto> LoginByRefreshToken(string refresh)
        {
            AppUser user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshToken == refresh);
            if (user is null) throw new Exception("Not found");
            if (user.RefreshTokenExpiredAt<DateTime.UtcNow) throw new Exception("Expired");

           TokenResponseDto tokendto= _handler.CreateJwt(user, await _createClaims(user), 60);

            user.RefreshToken = tokendto.RefreshToken;
            user.RefreshTokenExpiredAt = tokendto.RefreshExpireTime;
            await _userManager.UpdateAsync(user);

            return tokendto;

        }
    }
}
