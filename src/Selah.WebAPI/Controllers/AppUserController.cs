using System.Threading.Tasks;
using FluentValidation.Results;
using LanguageExt;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selah.Application.Commands.AppUser;
using Selah.Domain.Data.Models.Authentication;
using Selah.WebAPI.Extensions;

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
            Either<ValidationResult, AuthenticationResponse> result = await _mediatr.Send(command);
            return result.MapEitherToHttpResult(this);
        }
    }
}