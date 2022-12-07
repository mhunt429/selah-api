using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Selah.Domain.Data.Models.ApplicationUser;
using Selah.Domain.Data.Models.Authentication;

namespace Selah.Application.Services.Interfaces
{
  public interface IAuthenticationService
  {

    public JwtResponse GenerateJwt(Claim[] claims);

    public Guid? GetUserFromClaims(HttpRequest request);
  }
}