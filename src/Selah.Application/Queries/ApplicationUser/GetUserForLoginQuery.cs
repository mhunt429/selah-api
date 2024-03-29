﻿using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Selah.Application.Services.Interfaces;
using Selah.Domain.Data.Models.ApplicationUser;
using Selah.Domain.Data.Models.Authentication;
using Selah.Infrastructure.Repository.Interfaces;
using static BCrypt.Net.BCrypt;

namespace Selah.Application.Queries.ApplicationUser
{
    public class GetUserForLoginQuery : IRequest<AuthenticationResponse>
    {
        [DisplayName("emailOrUsername")]
        public string EmailOrUsername { get; set; }

        [DisplayName("password")]
        public string Password { get; set; }

        public sealed class Validator : AbstractValidator<GetUserForLoginQuery>
        {
            public Validator()
            {
                RuleFor(x => x.EmailOrUsername).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<GetUserForLoginQuery, AuthenticationResponse>
        {
            private readonly IAppUserRepository _appUserRepository;
            private readonly IAuthenticationService _authService;
            private readonly ISecurityService _securityService;
            public Handler(IAppUserRepository appUserRepository, IAuthenticationService authService, ISecurityService securityService)
            {
                _appUserRepository = appUserRepository;
                _authService = authService;
                _securityService = securityService;
            }

            public async Task<AuthenticationResponse> Handle(GetUserForLoginQuery query, CancellationToken cancellationToken)
            {
                var user = await _appUserRepository.GetUser(query.EmailOrUsername);
                if (user == null)
                {
                    return null;
                }

                if (!Verify(query.Password, user.Password))
                {
                    return null;
                }
                var userResponse = new UserViewModel
                {
                    Id = _securityService.EncodeHashId(user.Id),
                    Email = user.Email,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateCreated = user.DateCreated
                };
                var jwtResult = _authService.GenerateJwt(userResponse);

                return new AuthenticationResponse
                {
                    User = userResponse,
                    AccessToken = jwtResult.AccessToken,
                    ExpirationTs = jwtResult.ExpirationTs
                };
            }
        }
    }
}
