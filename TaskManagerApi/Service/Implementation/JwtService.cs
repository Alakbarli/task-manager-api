using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApi.Domain.Models;
using TaskManagerApi.DTO.ResponseModels;

namespace TaskManagerApi.Service.Interface
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }
        public JwtResponse GenerateToken(JwtCustomClaim claim)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                         new Claim("id", claim.Id),
                         new Claim("name", claim.Name),
                         new Claim("surname", claim.Surname),
                         new Claim("username", claim.Username),
                         new Claim("RoleName", claim.RoleName),
                         new Claim("OrganizationName", claim.OrganizationName),
                         new Claim("OrganizationId", claim.OrganizatioId.ToString()),
                         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                }),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            JwtResponse jwtresponse = new JwtResponse()
            {
                Token = tokenString,
            };

            return jwtresponse;
        }


    }
}
