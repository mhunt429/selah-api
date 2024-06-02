using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Selah.Application.Services.Interfaces;
using Selah.Domain.Data.Models.ApplicationUser;
using Selah.Infrastructure.Repository.Interfaces;

namespace Selah.Application.Queries.ApplicationUser;

public class GetUserByIdQuery : IRequest<UserViewModel>
{
    public  string Id { get; set; }

    public class Handler : IRequestHandler<GetUserByIdQuery, UserViewModel>
    {
        private readonly IAppUserRepository _userRepository;
        private readonly ISecurityService _securityService;

        public Handler(IAppUserRepository userRepository, ISecurityService securityService)
        {
            _userRepository = userRepository;
            _securityService = securityService;
        }

        public async Task<UserViewModel> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            //Get the plain text id by decoding the value found in the claims
            long id = _securityService.DecodeHashId(query.Id);
            AppUser userDto = await _userRepository.GetUser(id);
            if (userDto == null)
            {
                return null;
            }
            return new UserViewModel
            {
                Id = query.Id,
                Email = userDto.Email,
                UserName = userDto.UserName,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                DateCreated = userDto.DateCreated
            };
        }
    }
}