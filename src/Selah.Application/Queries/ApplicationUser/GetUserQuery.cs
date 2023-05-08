using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Selah.Domain.Data.Models.ApplicationUser;
using Selah.Infrastructure.Repository.Interfaces;
using static BCrypt.Net.BCrypt;

namespace Selah.Application.Queries.ApplicationUser
{
    public class GetUserQuery : IRequest<UserViewModel>
    {
        [DisplayName("emailOrUsername")]
        public string EmailOrUsername { get; set; }

        [DisplayName("password")]
        public string Password { get; set; }

        public sealed class Validator : AbstractValidator<GetUserQuery>
        {
            public Validator()
            {
                RuleFor(x => x.EmailOrUsername).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<GetUserQuery, UserViewModel>
        {
            private readonly IAppUserRepository _appUserRepository;
            private readonly IMapper _mapper;
            public Handler(IAppUserRepository appUserRepository)
            {
                _appUserRepository = appUserRepository;

            }

            public async Task<UserViewModel> Handle(GetUserQuery query, CancellationToken cancellationToken)
            {
                var user = await _appUserRepository.GetUser(query.EmailOrUsername);
                if (user == null) return null;

                if (!Verify(query.Password, user.Password))
                {
                    return null;
                }
                return new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateCreated = user.DateCreated
                };
            }
        }
    }
}
