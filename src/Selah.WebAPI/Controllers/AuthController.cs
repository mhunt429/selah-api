using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Selah.Application.Queries.ApplicationUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Selah.WebAPI.Extensions;


namespace Selah.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/oauth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediatr;

        public AuthController(IMediator mediatr)
        {
            _mediatr = mediatr;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] GetUserForLoginQuery forLoginQuery)
        {
            var response = await _mediatr.Send(forLoginQuery);
            if (response == null)
            {
                return Unauthorized();
            }

            return Ok(response);
        }

        [HttpGet("current-user")]
        public async Task<IActionResult> GetUserFromClaims()
        {
            var userId = HttpContext.Request.GetUserIdFromRequest();
            GetUserByIdQuery query = new GetUserByIdQuery { Id = userId };
            var result = await _mediatr.Send(query);
            return result != null ? Ok(result) : NotFound();
        }
    }
}