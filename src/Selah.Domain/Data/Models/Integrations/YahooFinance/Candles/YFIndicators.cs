using Newtonsoft.Json;
using System.Collections.Generic;

namespace Selah.Domain.Data.Models.Integrations.YahooFinance.Candles
{
  public  class YFIndicators
  {
    [JsonProperty("quote")]
    public List<YFQuote> Quote { get; set; }
  }
}
