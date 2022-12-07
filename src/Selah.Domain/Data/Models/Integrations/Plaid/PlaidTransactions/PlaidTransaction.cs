using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Selah.Domain.Data.Models.Plaid.PlaidTransactions
{
  public class PlaidTransaction
  {
    [JsonProperty("account_id")]
    public string AccountId { get; set; }
    
    [JsonProperty("amount")]
    public decimal Amount { get; set; }
    
    [JsonProperty("date")]
    public DateTime Date { get; set; }
    
    [JsonProperty("merchant_name")]
    public string MerchantName { get; set; }
    
    [JsonProperty("transaction_id")]
    public string TransactionId { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("pending")]
    public bool Pending { get; set; }
    
    [JsonProperty("payment_meta")]
    public PlaidTransactionPaymentMetadata? PaymentMeta { get; set; }
    
    [JsonProperty("category")]
    public IEnumerable<string> Category { get; set; }
  }
}