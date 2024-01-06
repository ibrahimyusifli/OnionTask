
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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
        public TokenResponseDto CreateToken(AppUser user,IEnumerable<Claim> claims, int minutes)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Lwt:SecurityKey"]));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(minutes),
                claims: claims,
                signingCredentials: credentials
                );
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            TokenResponsDto dto = new TokenResponsDto(handler.WriteToken(token), token.ValidTo, user.UserName, CreateRefreshToken(),token.ValidTo.AddMinutes(minutes/4));
            return dto
        }
        public string CreateRefreshToken()
        {
            return Guid.NewGuid().ToString();

            //byte[] bytes=new byte[32];
            //var random = RandomNumberGenerator.Create();
            //random.GetBytes(bytes);
            //return Convert.ToBase64String(bytes);
        }
    }
}
