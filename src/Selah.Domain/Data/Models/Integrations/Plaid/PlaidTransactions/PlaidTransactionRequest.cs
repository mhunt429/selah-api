using System;
using Newtonsoft.Json;

namespace Selah.Domain.Data.Models.Plaid.PlaidTransactions
{
  public class PlaidTransactionRequest
  {
    [JsonProperty("client_id")]
    public string ClientId { get; set; }
    
    [JsonProperty("secret")]
    public string Secret { get; set; }
    
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
    
    [JsonProperty("start_date")]
    public string StartDate { get; set; }
    
    [JsonProperty("end_date")]
    public string EndDate { get; set; }
    
    [JsonProperty("options")]
    public PlaidTransactionRequestOptions Options { get; set; }
  }
}