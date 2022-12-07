using Newtonsoft.Json;
using System.Collections.Generic;

namespace Selah.Domain.Data.Models.Integrations.YahooFinance.Candles
{

  public class YFQuote
  {
    [JsonProperty("quote")]
    public List<IndividualQuote> Quote { get; set; }
  }
  public class IndividualQuote
  {
    [JsonProperty("low")]
    public List<decimal?> Low { get; set; }

    [JsonProperty("volume")]
    public List<long?> Volume { get; set; }

    [JsonProperty("open")]
    public List<decimal?> Open { get; set; }

    [JsonProperty("high")]
    public List<decimal?> High { get; set; }

    [JsonProperty("close")]
    public List<decimal?> Close { get; set; }
  }
}
