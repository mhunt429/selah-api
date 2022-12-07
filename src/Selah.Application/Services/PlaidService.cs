using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Selah.Application.Services.Interfaces;
using Selah.Domain.Data.Models.Integrations.Plaid.PlaidAccounts;
using Selah.Domain.Data.Models.Plaid;
using Selah.Domain.Data.Models.Plaid.PlaidAccounts;
using Selah.Domain.Data.Models.Plaid.PlaidTransactions;
using Selah.Domain.Internal;
using Selah.Infrastructure.Repository.Interfaces;

namespace Selah.Application.Services
{
    public class PlaidService : IPlaidService
    {
        private readonly IHttpService _httpService;
        private readonly IOptions<EnvVariablesConfig> _envVariables;
        private readonly IUserInstitutionRepository _userInstitutionRepository;
        private readonly ILogger _logger;

        public PlaidService(IHttpService httpService,
          IOptions<EnvVariablesConfig> envVariables,
          IUserInstitutionRepository userInstitutionRepository,
          ILogger<PlaidService> logger
          )
        {
            _httpService = httpService;
            _envVariables = envVariables;
            _userInstitutionRepository = userInstitutionRepository;
            _logger = logger;

            _logger.LogInformation($"Creating REST configuration for {_envVariables.Value.PlaidEnv}.plaid.com");
        }
        public async Task<PlaidTokenLinkResponse> GetLinkToken()
        {
            _logger.LogInformation($"Creating REST configuration for {_envVariables.Value.PlaidEnv}.plaid.com");
            //_httpService = new HttpService( _envVariables, $"https://{_envVariables.Value.PlaidEnv}.plaid.com");
            var countryCodes = new List<string>();
            countryCodes.Add("US");
            var products = new List<string>();
            products.Add("auth");

            var data = new PlaidTokenLinkRequest
            {
                ClientId = _envVariables.Value.PlaidClientId,
                Secret = _envVariables.Value.PlaidClientSecret,
                ClientName = "Plaid App", //TODO move this to env variable
                CountryCodes = countryCodes,
                Language = "en",
                User = new PlaidTokenLinkUser { ClientUserId = "unique-per-user" },
                Products = products
            };
            try
            {
                var tokenLinkResponse = await _httpService.PostAsync<PlaidTokenLinkResponse>(data,
                  new Uri($"https://{_envVariables.Value.PlaidEnv}.plaid.com/link/token/create"));

                return tokenLinkResponse.Item1;
            }
            catch (Exception e)
            {
                _logger.LogError($"An an error occurred linking plaid token with error: ${e.Message}");
                return null;
            }
        }

        public async Task<PlaidTokenExchangeResponse> GetAccessToken(string publicToken)
        {
            var data = new PlaidTokenExchange
            {
                ClientId = _envVariables.Value.PlaidClientId,
                Secret = _envVariables.Value.PlaidClientSecret,
                PublicToken = publicToken
            };
            try
            {
                var tokenLinkExchange = await _httpService.PostAsync<PlaidTokenExchangeResponse>(data,
                  new Uri($"https://{_envVariables.Value.PlaidEnv}.plaid.com/item/public_token/exchange"));

                return tokenLinkExchange.Item1;
            }
            catch (Exception e)
            {
                _logger.LogError($"An an error occurred linking plaid token with error: ${e.Message}");
                return null;
            }
        }

        public async Task<PlaidAccountsResponse> ImportAccounts(PlaidAccountRequest request)
        {
            try
            {
                var accountsResponse = await _httpService.PostAsync<PlaidAccountsResponse>(request,
                  new Uri($"https://{_envVariables.Value.PlaidEnv}.plaid.com/accounts/get"));

                return accountsResponse.Item1;
            }
            catch (Exception e)
            {
                _logger.LogError($"An an error occurred imported Plaid accounts with error: ${e.Message}");
                return null;
            }
        }

        public async Task<PlaidTransactionsResponse> ImportTransactions(PlaidTransactionRequest request)
        {
            try
            {
               
                var transactionsResponse = await _httpService.PostAsync<PlaidTransactionsResponse>(request,
                  new Uri($"https://{_envVariables.Value.PlaidEnv}.plaid.com/transactions/get"));
                return transactionsResponse.Item1;
            }

            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace.ToString());
                return null;
            }

        }

        /*public void Dispose()
        {
          throw new NotImplementedException();
        }*/
    }
}