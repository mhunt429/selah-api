using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Selah.Application.Services.Interfaces;
using Selah.Domain.Data.Models.Authentication;
using Selah.Domain.Internal;

namespace Selah.Application.Services
{
    public class AuthenticationService: IAuthenticationService
  {
   private readonly IOptions<EnvVariablesConfig> _envVariables;

   public AuthenticationService(IOptions<EnvVariablesConfig> envVariables)
   {
     _envVariables = envVariables;
   }
    public JwtResponse GenerateJwt(Claim[] claims)
    {
      var jwtToken = new JwtSecurityToken(
        _envVariables.Value.JwtIssuer,
        string.Empty,
        claims,
        expires: DateTime.UtcNow.AddSeconds(85399),
        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(
          Encoding.ASCII.GetBytes(_envVariables.Value.JwtSecret)), SecurityAlgorithms.HmacSha256Signature));
      var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
      
      return new JwtResponse
      {
        AccessToken = accessToken,
        ExpirationTs = jwtToken.ValidTo
      };
    }

    public Guid? GetUserFromClaims(HttpRequest request)
    {
      if (request.Headers.TryGetValue("Authorization", out StringValues authToken))
      {
        var jwt = authToken.ToString().Replace("Bearer ", "");
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);
        return new Guid(token.Claims.First().Value);
      }

      return null;
    }
  }
}