using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Selah.Domain.Data.Models;
using Selah.Domain.Data.Models.Authentication;
using Selah.WebAPI.Shared;
using Selah.Domain.Data.Models.ApplicationUser;
using System.Collections.Generic;
using Selah.Domain.Data.Dictionaries;
using Selah.Application.Services.Interfaces;
using AutoMapper;
using MediatR;
using Selah.Application.Queries.ApplicationUser;

namespace Selah.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/oauth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediatr;
        public AuthController(IUserService userService, IAuthenticationService authService, IMapper mapper, IMediator mediatr)
        {
            _userService = userService;
            _authService = authService;
            _mapper = mapper;
            _mediatr = mediatr;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] GetUserQuery query)
        {
            var user = await _mediatr.Send(query);
            if (user == null)
            {
                return Unauthorized();
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
        public async Task<IActionResult> GetAuthenticatedUser()
        {
            try
            {
                if (Request.Headers.TryGetValue("Authorization", out StringValues authToken))
                {
                    var jwt = authToken.ToString().Replace("Bearer ", "");
                    var handler = new JwtSecurityTokenHandler();
                    var token = handler.ReadJwtToken(jwt);

                    var user = await _userService.GetUser(new Guid(token.Claims.First().Value));
                    if (user == null)
                    {
                        return Unauthorized(new HttpResponseViewModel<AppUser>
                        {
                            StatusCode = 401,

                        });
                    }
                    return Ok(_mapper.Map<UserViewModel>(user));
                }
                return Unauthorized(new HttpResponseViewModel<AppUser>
                {
                    StatusCode = 401,

                });
            }
            catch (Exception ex)
            {
                return BadRequest(new HttpResponseViewModel<AppUser>
                {
                    StatusCode = 400,

                });
            }
        }
    }
}