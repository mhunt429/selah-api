using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Selah.Domain.Data.Models.Authentication;
using Selah.Domain.Internal;

namespace Selah.WebAPI.Middleware
{

    public static class JwtConfiguration
  {
     static readonly IOptions<EnvVariablesConfig> _envVariables;

    static JwtConfiguration()
    {
      _envVariables = Options.Create(new EnvVariablesConfig());
    }

    public static void ConfigureJwt(IServiceCollection services)
    {
      var jwtTokenConfig = new JwtConfig
      {
        AccessTokenExpiration = 85399,
        Issuer = _envVariables.Value.JwtIssuer,
        Secret = _envVariables.Value.JwtSecret
      };
      services.AddSingleton(jwtTokenConfig);
      services.AddAuthentication(x =>
      {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(x =>
      {
        x.RequireHttpsMetadata = true;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_envVariables.Value.JwtSecret)),
          ValidateIssuer = false,

          ValidateAudience = false,
          ClockSkew = TimeSpan.FromMinutes(1)
        };
      });
    }
  }
}