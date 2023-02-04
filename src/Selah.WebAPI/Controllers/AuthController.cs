using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Selah.Domain.Data.Models;
using Selah.Domain.Data.Models.Authentication;
using Selah.Domain.Data.Models.ApplicationUser;
using Selah.Application.Services.Interfaces;
using AutoMapper;
using MediatR;
using Selah.Application.Queries.ApplicationUser;
using Microsoft.AspNetCore.Authorization;

namespace Selah.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/oauth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authService;
        private readonly IMediator _mediatr;
        public AuthController(IUserService userService, IAuthenticationService authService, IMediator mediatr)
        {
            _userService = userService;
            _authService = authService;
            _mediatr = mediatr;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] GetUserQuery query)
        {
            var user = await _mediatr.Send(query);
            if (user == null)
            {
                return Unauthorized(new HttpResponseViewModel<AppUser>
                {
                    StatusCode = 401,

                });
            }

            var claims = new[]
            {
        new Claim(ClaimTypes.Name,user.Id.ToString()),
      };

            var jwtResult = _authService.GenerateJwt(claims);

            return Ok(new AuthenticationResponse
            {
                User = user,
                AccessToken = jwtResult.AccessToken,
                ExpirationTs = jwtResult.ExpirationTs
            });
        }

        [HttpGet("current-user")]
        public async Task<ActionResult<HttpResponseViewModel<UserViewModel>>> GetAuthenticatedUser()
        {

            if (Request.Headers.TryGetValue("Authorization", out StringValues authToken))
            {
                var jwt = authToken.ToString().Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwt);

                var user = await _userService.GetUser(new Guid(token.Claims.First().Value));
                if (user == null)
                {
                    return Forbid();
                }
                return Ok(new HttpResponseViewModel<UserViewModel>
                {
                    StatusCode = 200,
                    Data = new System.Collections.Generic.List<UserViewModel> { new UserViewModel
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        DateCreated = user.DateCreated
                    }}
                });
            }
            return Forbid();
        }
    }
}