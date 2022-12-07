using System.Collections;
using System.Threading.Tasks;
using Selah.Domain.Data.Models.Integrations.Plaid.PlaidAccounts;
using Selah.Domain.Data.Models.Plaid;
using Selah.Domain.Data.Models.Plaid.PlaidAccounts;
using Selah.Domain.Data.Models.Plaid.PlaidTransactions;

namespace Selah.Application.Services.Interfaces
{
  public interface IPlaidService
  {
    /// <summary>
    /// Used to get a public token to be exchanged for an access token 
    /// </summary>
    /// <returns></returns>
    public Task<PlaidTokenLinkResponse> GetLinkToken();

    public Task<PlaidTokenExchangeResponse> GetAccessToken(string publicToken);

    public Task<PlaidAccountsResponse> ImportAccounts(PlaidAccountRequest request);

    public Task<PlaidTransactionsResponse> ImportTransactions(PlaidTransactionRequest request);
  }
}