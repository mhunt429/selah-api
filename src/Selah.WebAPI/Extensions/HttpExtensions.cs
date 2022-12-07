using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Selah.WebAPI.Extensions
{
    public static class HttpExtensions
    {
        public static Guid GetUserIdFromRequest(this HttpRequest request)
        {
            if (request.Headers.TryGetValue("Authorization", out StringValues authToken))
            {
                var jwt = authToken.ToString().Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwt);
                return new Guid(token.Claims.First().Value);
            }

            return Guid.Empty;
        }
    }
}
