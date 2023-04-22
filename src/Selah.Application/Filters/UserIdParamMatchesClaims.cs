using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Selah.Application.Filters
{
    /// <summary>
    /// A custom authorization filter that checks if the user id in the JWT token's claims matches the userId present in the route parameters.
    /// </summary>
    public class UserIdParamMatchesClaims : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// Performs authorization by checking if the user id in the JWT token's claims matches the userId present in the route parameters.
        /// </summary>
        /// <param name="context">The context for the authorization filter.</param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //Get the authorization header, if not present return 403
            if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues authToken))
            {
                var jwt = authToken.ToString().Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwt);
                string subject = token.Claims.First().Value;
                string userIdRoute = context.RouteData.Values["userId"].ToString();

                //If user id from claims does not equal the userId present on route parameters, return 403
                if (subject == userIdRoute)
                {
                    return;
                }
                context.Result = new ForbidResult();
            }
            else
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
