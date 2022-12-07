using System.Collections.Generic;
using System.Text.Json.Serialization;
using Selah.Domain.Data.Models.Integrations.Plaid.PlaidAccounts;

namespace Selah.Domain.Data.Models.Plaid.PlaidAccounts
{
  public class PlaidAccountsResponse
  {
    [JsonPropertyName("accounts")]
    public IEnumerable<PlaidAccount> Accounts { get; set; }
  }
}