using MediatR;
using Selah.Domain.Data.Models.ApplicationUser;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using FluentValidation;
using FluentValidation.Results;
using Selah.Application.Services.Interfaces;
using Selah.Infrastructure.Repository.Interfaces;

namespace Selah.Application.Commands.AppUser
{
    public class CreateUserCommand : IRequest<(UserViewModel, ValidationResult)>
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
        public class Handler : IRequestHandler<CreateUserCommand, (UserViewModel, ValidationResult)>
        {
            private readonly IAppUserRepository _appUserRepository;
            private readonly ISecurityService _securityService;
            public Handler(IAppUserRepository appUserRepository, ISecurityService securityService)
            {
                _appUserRepository = appUserRepository;
                _securityService = securityService;
            }

            public async Task<(UserViewModel, ValidationResult)> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                var validator = new Validator(_appUserRepository);

                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return (null, validationResult);
                }
                request.CreatedUser.Password = BCrypt.Net.BCrypt.HashPassword(request.CreatedUser.Password);
                int userId = await _appUserRepository.CreateUser(request.CreatedUser);
           
                var user = new UserViewModel
                {
                    Id = _securityService.EncodeHashId(userId),
                    Email = request.CreatedUser.Email,
                    UserName = request.CreatedUser.UserName,
                    FirstName = request.CreatedUser.FirstName,
                    LastName = request.CreatedUser.LastName,
                    DateCreated = request.CreatedUser.DateCreated,
                };

                return (user, null);
            }
        }
    }
}
