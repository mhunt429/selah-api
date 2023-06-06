using Selah.Application.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace Selah.Application.UnitTests.Filters
{
    public class UserIdParamMatchesClaimsTests
    {
        private readonly UserIdParamMatchesClaims _filter;
        private Mock<AuthorizationFilterContext> _context = new Mock<AuthorizationFilterContext>();
        public UserIdParamMatchesClaimsTests()
        {
            _filter = new UserIdParamMatchesClaims();
        }

        [Fact]
        public void OnAuthorization_Should_Return_ForbidResult_When_User_Id_Does_Not_Match_Route_Parameter()
        {
            // Arrange
            var context = new AuthorizationFilterContext(CreateActionContext(), new List<IFilterMetadata>());
            var claims = new List<Claim>
        {
            new Claim("sub", "wronguserid")
        };
            var jwt = CreateJwtSecurityToken(claims);
            context.HttpContext.Request.Headers.Add("Authorization", new StringValues($"Bearer {jwt}"));
            context.RouteData.Values.Add("userId", "correctuserid");

            // Act
            _filter.OnAuthorization(context);

            // Assert
            Assert.IsType<ForbidResult>(context.Result);
        }

        [Fact]
        public void OnAuthorization_Should_Not_Set_Result_When_User_Id_Matches_Route_Parameter()
        {
            // Arrange
            var context = new AuthorizationFilterContext(CreateActionContext(), new List<IFilterMetadata>());
            var claims = new List<Claim>
        {
            new Claim("sub", "correctuserid")
        };
            var jwt = CreateJwtSecurityToken(claims);
            context.HttpContext.Request.Headers.Add("Authorization", new StringValues($"Bearer {jwt}"));
            context.RouteData.Values.Add("userId", "correctuserid");

            // Act
            _filter.OnAuthorization(context);

            // Assert
            Assert.Null(context.Result);
        }

        private static ActionContext CreateActionContext()
        {
            var context = new ActionContext(
                new DefaultHttpContext(),
                new RouteData(),
                new ActionDescriptor()
            );

            return context;
        }

        private static string CreateJwtSecurityToken(IEnumerable<Claim> claims)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(new byte[32]), SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(securityToken);
        }
    }
}
