using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selah.Application.Commands.AppUser;
using Selah.Application.Services.Interfaces;
using Selah.Domain.Data.Dictionaries;
using Selah.Domain.Data.Models;
using Selah.Domain.Data.Models.ApplicationUser;
using Selah.WebAPI.Extensions;
using Selah.Application.Filters;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Selah.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/users")]
    public class AppUserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMediator _mediatr;

        public AppUserController(IUserService userService, IMediator mediator)
        {
            _userService = userService;
            _mediatr = mediator;
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
            return Ok(user);

        }
        //Sign up Endpoint
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommand command)
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