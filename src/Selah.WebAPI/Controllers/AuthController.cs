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
using Microsoft.Net.Http.Headers;

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
            var response = await _mediatr.Send(query);
            if (response == null)
            {
                return Unauthorized();
            }
            return Ok(response);
        }
    }
}