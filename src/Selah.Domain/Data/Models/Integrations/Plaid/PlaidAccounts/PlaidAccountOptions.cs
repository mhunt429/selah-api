using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Selah.Domain.Data.Models.Integrations.Plaid.PlaidAccounts
{
  public class PlaidAccountOptions
  {
    [JsonPropertyName("account_ids")]
    public List<string> AccountIds { get; set; }
  }
}