using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Selah.Application.Services.Interfaces;
using Selah.Domain.Data.Models.Integrations;
using Selah.Domain.Data.Models.Integrations.TDAmeritrade;
using Selah.Domain.Data.Models.Integrations.YahooFinance.Candles;
using Selah.Domain.Data.Models.Integrations.YahooFinance.Quote;
using Selah.Domain.Data.Models.Investments;
using Selah.Domain.Internal;

namespace Selah.Application.Services
{
    public class InvestmentService : IInvestmentService
    {
        private IHttpService _httpService;
        private readonly IOptions<EnvVariablesConfig> _envVariables;
        private readonly IAuthorizedAppService _authorizedAppService;
        private readonly ISecurityService _securityService;
        public InvestmentService(
          IHttpService httpService,
          IOptions<EnvVariablesConfig> envVariables,
          IAuthorizedAppService authorizedAppService,
          ISecurityService securityService)
        {
            _httpService = httpService;
            _envVariables = envVariables;
            _authorizedAppService = authorizedAppService;
            _securityService = securityService;
        }

        #region TD Ameritrade
        /// <summary>
        /// TDAmeritrade allows unauthenticated requests, the data just won't be as up to date so returning
        /// a null access token is perfectly accessible to allow the app to function properly
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<string> CheckAndRefreshAuthentication(Guid userId)
        {
            var currentCredentials = await _authorizedAppService.GetOAuthCredentials("TDAmeritrade", userId);
            if (currentCredentials == null)
            {
                return null;
            }
            if ((currentCredentials.AccessTokenExpirationTs - DateTime.UtcNow).Minutes > 5)
            {
                return currentCredentials.EncryptedAccessToken;
            }

            try
            {
                var updatedToken =
                  await RefreshTdAmeritradeAuthToken(await _securityService.Decrypt(currentCredentials.EncryptedRefreshToken));

                var authorizedAppToSave = new UserAuthorizedApp
                {
                    UserId = userId,
                    EncryptedAccessToken = await _securityService.Encrypt(updatedToken.AccessToken),
                    EncryptedRefreshToken = currentCredentials.EncryptedRefreshToken,
                    AccessTokenExpirationTs = DateTime.UtcNow.AddSeconds(updatedToken.ExpiresIn),
                    RefreshTokenExpirationTs = currentCredentials.RefreshTokenExpirationTs,
                    Scope = "TDAmeritrade",
                    CreatedTs = currentCredentials.CreatedTs,
                    UpdatedTs = DateTime.UtcNow,
                };
                await _authorizedAppService.UpdateOAuthCredentials(authorizedAppToSave);
                return await _securityService.Encrypt(updatedToken.AccessToken);
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<TimeSeriesVM> GetHistoryByTicker(TimeSeriesRequest request, Guid userId)
        {
            var accessToken = await _securityService.Decrypt(await CheckAndRefreshAuthentication(userId));

            if (!string.IsNullOrEmpty(accessToken))
            {
                _httpService.AddBearerToken(accessToken);
            }

            var apiKeyString = string.IsNullOrEmpty(accessToken) ? $"?apikey={_envVariables.Value.TDAmeritradeAPIKey}&" : "?";

            var response = await _httpService.GetAsync<AssetHistoryResponse>(new Uri(
              $@"https://api.tdameritrade.com/v1/marketdata/{request.Ticker}/pricehistory{apiKeyString}periodType={request.Period}&frequencyType={request.FrequencyType}"));
            if (response.Item1.Candles == null)
            {
                return null;
            }

            var timeSeries = new TimeSeriesVM
            {
                Series = response.Item1.Candles.Select(ts => new TimeSeries
                {
                    Close = ts.Close,
                    Date = ts.ToDate(),
                    Low = ts.Low,
                    Open = ts.Open,
                    Volume = ts.Volume
                }).ToList()
            };

            return timeSeries;
        }

        //https://auth.tdameritrade.com/auth?response_type=code&redirect_uri=https%3A%2F%2F127.0.0.1%3A8080&client_id=ABC1234%40AMER.OAUTHAP
        public string GetTdAmeritradeAuthUrl()
        {
            return
              $"https://auth.tdameritrade.com/auth?response_type=code&redirect_uri=http://127.0.0.1:4200&client_id={_envVariables.Value.TDAmeritradeAPIKey}%40AMER.OAUTHAP";
        }

        public async Task<OAuthTokenResponse> GetTdAmeritradeAuthToken(string authCode)
        {
            var formData = new Dictionary<string, string>
      {
        { "grant_type", "authorization_code" },
        {"access_type", "offline"},
        {"code", authCode},
        {"client_id", _envVariables.Value.TDAmeritradeAPIKey},
        {"redirect_uri", "http://127.0.0.1:4200"}// TODO add this to env variable, this is just for testing
      };
            var tokenResponse = await _authorizedAppService.GetOAuthAccessToken<OAuthTokenResponse>(
              new Uri("https://api.tdameritrade.com/v1/oauth2/token"), null, new FormUrlEncodedContent(formData));

            return tokenResponse;
        }

        public async Task<OAuthTokenResponse> RefreshTdAmeritradeAuthToken(string refreshToken)
        {
            var formData = new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                {"refresh_token", refreshToken},
                {"client_id", _envVariables.Value.TDAmeritradeAPIKey},
            };
            var tokenResponse = await _authorizedAppService.RefreshOAuthAccessToken<OAuthTokenResponse>(
              new Uri("https://api.tdameritrade.com/v1/oauth2/token"), null, new FormUrlEncodedContent(formData));

            return tokenResponse;
        }

        public async Task<List<TDAmeritradeAccounts>> GetTdAmeritradeAccounts(Guid userId)
        {
            var accessToken = await _securityService.Decrypt(await CheckAndRefreshAuthentication(userId));

            if (!string.IsNullOrEmpty(accessToken))
            {
                _httpService.AddBearerToken(accessToken);
            }

            var response = await _httpService.GetAsync<List<TDAmeritradeAccounts>>(new Uri("https://api.tdameritrade.com/v1/accounts"));
            var accounts = response.Item1;
            return accounts;
        }
        #endregion TD Ameritrade
    }
}

