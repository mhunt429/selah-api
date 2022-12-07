using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selah.Application.Services.Interfaces;
using Selah.Domain.Data.Models;
using Selah.Domain.Data.Models.ApplicationUser;
using Selah.Domain.Data.Models.Plaid;

namespace Selah.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/banking")]
    public class BankingController : ControllerBase
    {
        private readonly IBankingService _bankingService;
        private readonly IAuthenticationService _authService;
        public BankingController(IBankingService bankingService, IAuthenticationService authService)
        {
            _bankingService = bankingService;
            _authService = authService;
        }

        [HttpPost("link-institution")]
        public async Task<IActionResult> LinkInstitution([FromBody] PlaidAccountLinkRequest institution)
        {
            try
            {
                var userId =
                  _authService.GetUserFromClaims(Request);
                if (userId == null)
                {
                    return Unauthorized();
                }
                institution.User.UserId = userId.Value;
                var linkedInstitution = await _bankingService.CreateUserInstitution(institution);
                await _bankingService.ImportAccounts(linkedInstitution.Id);
                return Ok(linkedInstitution);
            }
            catch (Exception e)
            {
                return BadRequest(new HttpResponseViewModel<AppUser>
                {
                    StatusCode = 400,

                });
            }
        }

        [HttpPost("import-accounts")]
        public async Task<IActionResult> ImportAccounts([FromQuery(Name = "institutionId")] Guid institutionId)
        {
            //TODO add more validation on this method but this is just testing for now
            try
            {
                await _bankingService.ImportAccounts(institutionId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> GetAccounts()
        {
            var userId = _authService.GetUserFromClaims(Request);
            if (userId == null)
            {
                return Unauthorized();
            }
            var accounts = await _bankingService.GetAccounts(userId);
            return Ok(accounts);
        }
    }
}