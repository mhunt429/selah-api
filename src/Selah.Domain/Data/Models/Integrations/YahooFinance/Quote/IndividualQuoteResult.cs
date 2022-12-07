using Newtonsoft.Json;

namespace Selah.Domain.Data.Models.Integrations.YahooFinance.Quote
{
  public class IndividualQuoteResult
  {
    public string ShortName { get; set; }

    public string LongName { get; set; }

    public string Symbol { get; set; }

    [JsonProperty("regularMarketPrice")]
    public decimal? MarketPrice { get; set; }

    [JsonProperty("bid")]
    public decimal? CurrentPrice { get; set; }

    [JsonProperty("regularMarketPreviousClose")]
    public decimal PreviousClose { get; set; }
  }
}
