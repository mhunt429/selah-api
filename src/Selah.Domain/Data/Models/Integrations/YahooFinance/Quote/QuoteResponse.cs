using Newtonsoft.Json;
using System.Collections.Generic;

namespace Selah.Domain.Data.Models.Integrations.YahooFinance.Quote
{
  public class QuoteResponse
  {
    [JsonProperty("quoteResponse")]
    public QuoteResult Data { get; set; }
  }

  public class QuoteResult
  {
    [JsonProperty("result")]
    public IEnumerable<IndividualQuoteResult> Result { get; set; }
  }
}
