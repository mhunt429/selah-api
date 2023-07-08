using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Selah.Application.Services.Interfaces;
using Selah.Domain.Data.Models.ApplicationUser;
using Selah.Domain.Data.Models.Authentication;
using Microsoft.Extensions.Configuration;


namespace Selah.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        public AuthenticationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public JwtResponse GenerateJwt(UserViewModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_configuration["JWT_SECRET"]);

            var claims = new List<Claim>
            {
                new ( JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddSeconds(86400),
                Issuer = _configuration["JWT_ISSUER"],
                Audience = "selah-api",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var jwt = tokenHandler.WriteToken(token);

            return new JwtResponse
            {
                AccessToken = jwt,
                ExpirationTs = token.ValidTo
            };
        }

        public string GetUserFromClaims(HttpRequest request)
        {
            if (request.Headers.TryGetValue("Authorization", out StringValues authToken))
            {
                var jwt = authToken.ToString().Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwt);
                return token.Claims.First().Value;
            }

            return "";
        }
    }
}