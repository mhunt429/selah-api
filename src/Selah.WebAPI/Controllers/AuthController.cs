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
        private readonly IAuthenticationService _authService;
        private readonly IMediator _mediatr;
        public AuthController(IAuthenticationService authService, IMediator mediatr)
        {

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
            var jwtResult = _authService.GenerateJwt(user);

            return Ok(new AuthenticationResponse
            {
                User = user,
                AccessToken = jwtResult.AccessToken,
                ExpirationTs = jwtResult.ExpirationTs
            });
        }
    }
}