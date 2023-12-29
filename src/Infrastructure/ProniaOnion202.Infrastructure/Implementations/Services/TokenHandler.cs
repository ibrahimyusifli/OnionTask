
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion202.Infrastructure.Implementations.Services
{
    public class TokenHandler:ITokenHandler
    {

        private readonly IConfiguration _config;

        public TokenHandler(IConfiguration config)
        {
            _config = config;
        }
        public TokenResponseDto CreateToken(AppUser user, int minutes)
        {
            ICollection<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.GivenName,user.Name),
                new Claim(ClaimTypes.Surname,user.Surname)
            };
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Lwt:SecurityKey"]));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(60),
                claims: claims,
                signingCredentials: credentials
                );
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
           
            return new TokenResponseDto ( handler.WriteToken(token),user.UserName,token.ValidTo);
        }
    }
}
