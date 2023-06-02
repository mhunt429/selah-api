using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Selah.Domain.Data.Models.Authentication;

namespace Selah.WebAPI.Middleware
{

    public static class JwtConfiguration
    {
        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtTokenConfig = new JwtConfig
            {
                AccessTokenExpiration = 86400,
                Issuer = configuration["JWT_ISSUER"],
                Secret = configuration["JWT_SECRET"]
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT_SECRET"])),
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWT_ISSUER"],
                    ValidateAudience = false,

          
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });
        }
    }
}