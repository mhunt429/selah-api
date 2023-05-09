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
        private readonly IMediator _mediatr;

        public AppUserController(IUserService userService, IMediator mediator)
        {

            _mediatr = mediator;
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
    }
}