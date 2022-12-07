using MediatR;
using Selah.Domain.Data.Models.ApplicationUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using System.ComponentModel;
using FluentValidation;
using global::Selah.Infrastructure.Repository.Interfaces;

namespace Selah.Application.Commands.AppUser
{
    public class CreateUserCommand : IRequest<UserViewModel>
    {
        [DisplayName("createdUser")]
        public AppUserCreate CreatedUser { get; set; }

        public sealed class Validator : AbstractValidator<CreateUserCommand>
        {
            private readonly IAppUserRepository _appUserRepository;

            public Validator(IAppUserRepository appUserRepository)
            {
                _appUserRepository = appUserRepository;

                RuleFor(x => x.CreatedUser.Email).NotEmpty();
                RuleFor(x => x.CreatedUser.Password).NotEmpty();
                RuleFor(x => x.CreatedUser.FirstName).NotEmpty();
                RuleFor(x => x.CreatedUser.LastName).NotEmpty();
                RuleFor(x => x.CreatedUser.Email).MustAsync(async (email, cancellation) =>
                {
                    var user = await _appUserRepository.GetUser(email);
                    return user == null;
                }).WithMessage("An account with this email already exists.");
            }
        }
        public class Handler : IRequestHandler<CreateUserCommand, UserViewModel>
        {
            private readonly IAppUserRepository _appUserRepository;
            private readonly IMapper _mapper;

            public Handler(IAppUserRepository appUserRepository, IMapper mapper)
            {
                _appUserRepository = appUserRepository;
                _mapper = mapper;
            }

            public async Task<UserViewModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                request.CreatedUser.Password = BCrypt.Net.BCrypt.HashPassword(request.CreatedUser.Password);
                Guid userId = await _appUserRepository.CreateUser(request.CreatedUser);
                var user = new Domain.Data.Models.ApplicationUser.AppUser
                {
                    Id = userId,
                    Email = request.CreatedUser.Email,
                    UserName = request.CreatedUser.UserName,
                    FirstName = request.CreatedUser.FirstName,
                    LastName = request.CreatedUser.LastName,
                    DateCreated = DateTime.UtcNow,
                };

                return _mapper.Map<UserViewModel>(user);
            }
        }
    }
}
