using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Selah.Domain.Data.Models.Investments;
using Microsoft.AspNetCore.Authorization;
using Selah.Domain.Data.Models.Integrations;
using Selah.Domain.Data.Models.Integrations.TDAmeritrade;
using Selah.Application.Services.Interfaces;

namespace Selah.WebAPI
{
    [ApiController]
    [Authorize]
    [Route("api/v1/users/{userId}/investments")]
    public class InvestmentsController : ControllerBase
    {
        private readonly IInvestmentService _investmentService;
        private IAuthorizedAppService _authorizedAppService;
        private readonly IAuthenticationService _authService;
        private readonly ISecurityService _securityService;

        public InvestmentsController(
          IInvestmentService investmentService,
          IAuthorizedAppService authorizedAppService,
          IAuthenticationService authService,
          ISecurityService securityService)
        {
            _investmentService = investmentService;
            _authorizedAppService = authorizedAppService;
            _authService = authService;
            _securityService = securityService;
        }
        [HttpGet("time-series")]
        public async Task<IActionResult> GetHistoryByTicker(string ticker, string period, string frequencyType)
        {
            var userId = _authService.GetUserFromClaims(Request);
            if (userId == null)
            {
                return Unauthorized();
            }
            var timeSeries = await _investmentService.GetHistoryByTicker(new TimeSeriesRequest { Ticker = ticker, Period = period, FrequencyType = frequencyType }, userId.Value);
            if (timeSeries == null)
            {
                return BadRequest("An error occured fetching time series data");
            }
            return Ok(timeSeries);
        }

        [HttpGet("authorize")]
        public IActionResult AuthorizeTdAmeritrade()
        {
            var authorizationUrl = _investmentService.GetTdAmeritradeAuthUrl();
            return Ok(new { authorizationUrl });
        }

        [HttpPost("access_token")]
        public async Task<IActionResult> GetOAuthAccessToken([FromBody] OAuthTokenRequest request)
        {
            var userId = _authService.GetUserFromClaims(Request);
            if (userId == null)
            {
                return Unauthorized();
            }
            try
            {
                var accessTokenResponse = await _investmentService.GetTdAmeritradeAuthToken(request.Code);
                if (accessTokenResponse == null)
                {
                    return BadRequest("An error occurred linking TDAmeritrade");
                }
                var credentialsToSave = new UserAuthorizedAppCreate
                {
                    UserId = userId.Value,
                    Scope = "TDAmeritrade",
                    EncryptedAccessToken = await _securityService.Encrypt(accessTokenResponse.AccessToken),
                    EncryptedRefreshToken = await _securityService.Encrypt(accessTokenResponse.RefreshToken),
                    AccessTokenExpirationTs = DateTime.UtcNow.AddSeconds(accessTokenResponse.ExpiresIn),
                    RefreshTokenExpirationTs = DateTime.UtcNow.AddSeconds(accessTokenResponse.RefreshTokenExpiresIn),
                    CreatedTs = DateTime.UtcNow,
                    UpdatedTs = DateTime.UtcNow
                };

                var saveCredentials = await _authorizedAppService.CreateOAuthCredentials(credentialsToSave); //TODO remove this placeholder
                if (saveCredentials == null)
                {
                    return NoContent();
                }
                return BadRequest("An error occurred linking TDAmeritrade");
            }
            catch (Exception e)
            {
                return BadRequest($"An error occurred linking TDAmeritrade");
            }
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> GetInvestmentAccounts(Guid userId)
        {
            var accounts = await _investmentService.GetTdAmeritradeAccounts(userId);
            return Ok(accounts);
        }
    }
}