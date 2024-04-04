using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selah.Application.Commands.AppUser;

namespace Selah.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/users")]
    public class AppUserController : ControllerBase
    {
        private readonly IMediator _mediatr;

        public AppUserController(IMediator mediator)
        {

            _mediatr = mediator;
        }

        //Sign up Endpoint
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            var result = await _mediatr.Send(command);
            if (result.IsRight)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}