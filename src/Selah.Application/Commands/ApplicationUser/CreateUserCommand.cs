using System;
using MediatR;
using Selah.Domain.Data.Models.ApplicationUser;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Selah.Application.Services.Interfaces;
using Selah.Domain.Data.Models.Authentication;
using Selah.Infrastructure.Repository.Interfaces;

namespace Selah.Application.Commands.AppUser
{
    public class CreateUserCommand : IRequest<(AuthenticationResponse, ValidationResult)>
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public sealed class Validator : AbstractValidator<CreateUserCommand>
        {
            private readonly IAppUserRepository _appUserRepository;

            public Validator(IAppUserRepository appUserRepository)
            {
                _appUserRepository = appUserRepository;

                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.FirstName).NotEmpty();
                RuleFor(x => x.LastName).NotEmpty();
                RuleFor(x => x.Email).MustAsync(async (email, cancellation) =>
                {
                    var user = await _appUserRepository.GetUser(email);
                    return user == null;
                }).WithMessage("An account with this email already exists.");
            }
        }

        public class Handler : IRequestHandler<CreateUserCommand, (AuthenticationResponse, ValidationResult)>
        {
            private readonly IAppUserRepository _appUserRepository;
            private readonly ISecurityService _securityService;
            private readonly IAuthenticationService _authenticationService;

            public Handler(IAppUserRepository appUserRepository, ISecurityService securityService,
                IAuthenticationService authenticationService)
            {
                _appUserRepository = appUserRepository;
                _securityService = securityService;
                _authenticationService = authenticationService;
            }

            public async Task<(AuthenticationResponse, ValidationResult)> Handle(CreateUserCommand request,
                CancellationToken cancellationToken)
            {
                var validator = new Validator(_appUserRepository);

                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return (null, validationResult);
                }

                request.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
                int userId = await _appUserRepository.CreateUser(new AppUserCreate
                {
                    Email = request.Email,
                    UserName = request.UserName,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    DateCreated = request.DateCreated
                });

                var user = new UserViewModel
                {
                    Id = _securityService.EncodeHashId(userId),
                    Email = request.Email,
                    UserName = request.UserName,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    DateCreated = request.DateCreated,
                };
                var jwtResult = _authenticationService.GenerateJwt(user);


                return (new AuthenticationResponse
                {
                    User = user, AccessToken = jwtResult.AccessToken,
                    ExpirationTs = jwtResult.ExpirationTs
                }, null);
            }
        }
    }
}