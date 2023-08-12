using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selah.Application.Commands.AppUser;
using Selah.Domain.Data.Models.ApplicationUser;
using Selah.WebAPI.Extensions;
using ValidationResult = FluentValidation.Results.ValidationResult;

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
            switch (result)
            {
                case (UserViewModel user, null):
                    return Ok(user);
                case (null, ValidationResult validationResult):
                    return BadRequest(validationResult.GetValidationErrors());
                default:
                    return BadRequest();
            }
        }
    }
}