using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Selah.Application.Commands.AppUser;
using Selah.Application.Services.Interfaces;
using Selah.Domain.Data.Dictionaries;
using Selah.Domain.Data.Models;
using Selah.Domain.Data.Models.ApplicationUser;
using Selah.WebAPI.Extensions;
using Selah.WebAPI.Shared;
using Selah.Application.Filters;

namespace Selah.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/users")]
    public class AppUserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediatr;

        private readonly IValidator<CreateUserCommand> _createUserValidator;

        public AppUserController(IUserService userService, IMapper mapper, IMediator mediator, IValidator<CreateUserCommand> createUserValidator)
        {
            _userService = userService;
            _mapper = mapper;
            _mediatr = mediator;
            _createUserValidator = createUserValidator;
        }


        [UserIdParamMatchesClaims]
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserViewModel>> GetUserById(Guid userId)
        {
            var user = await _userService.GetUser(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<UserViewModel>(user));

        }
        //Sign up Endpoint
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserViewModel>> CreateUser(CreateUserCommand command)
        {
            var validationResult = await _createUserValidator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                return BadRequest(new HttpResponseViewModel<UserViewModel>
                {
                    StatusCode = 400,
                    Errors = validationResult.GetValidationErrors()
                });
            }
            return Ok(await _mediatr.Send(command));
        }
        //TODO validate on user id 
        [UserIdParamMatchesClaims]
        [HttpPut("{userId}/update-password")]
        public async Task<IActionResult> UpdatePassword(Guid id, [FromBody] PasswordUpdate passwordUpdate)
        {
            var userId = Request.GetUserIdFromRequest();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }


            if (!await _userService.UpdatePassword(id, passwordUpdate))
            {
                var errors = new List<ErrorMessage>();
                errors.Add(new ErrorMessage
                {
                    Key = nameof(HttpErrorKeys.PasswordResetMismatch),
                    Message = "Updated password and password confirmation do not match."
                });
                return Unauthorized();
            }

            return NoContent();
        }
    }
}