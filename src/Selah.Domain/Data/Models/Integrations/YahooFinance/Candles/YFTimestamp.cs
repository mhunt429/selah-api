using Newtonsoft.Json;
using System.Collections.Generic;

namespace Selah.Domain.Data.Models.Integrations.YahooFinance.Candles
{
  public class YFTimestamp
  {
    [JsonProperty("timestamp")]
    public List<long> Timestamp { get; set; }
  }
}
