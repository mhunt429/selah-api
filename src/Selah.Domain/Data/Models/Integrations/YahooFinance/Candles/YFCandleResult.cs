using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Selah.Domain.Data.Models.Integrations.YahooFinance.Candles
{
  public class YFCandleChart
  {
    [JsonProperty("chart")]
    public YFCandleResult Chart { get; set; }

    public TimeSeriesVM MapToTimeSeriesVM()
    {
  
      var series = new List<TimeSeries>();
      var data = Chart.Result.FirstOrDefault();
      var indicators = data.Indicators.Quote.FirstOrDefault();

      for (var i = 0; i < data.Timestamp.Count; i++)
      {
        series.Add(new TimeSeries
        {
          //TODO might want to add some validation in case millis is returned rather than seconds
          Date = DateTimeOffset.FromUnixTimeMilliseconds(data.Timestamp[i] * 1000).UtcDateTime,
          Close = indicators.Close[i] != null ? indicators.Close[i].Value : null,
          Open = indicators.Open[i] != null ? indicators.Open[i].Value : null,
          Low = indicators.Low[i] != null ? indicators.Low[i].Value : null,
          Volume = indicators.Volume[i] != null ? indicators.Volume[i].Value : null,
        });
      }

      TimeSeriesVM updatedModel = new TimeSeriesVM {
        Series = series.Where(s => s.Close != null).ToList() 
      };

      return updatedModel;
    }
  }
  public class YFCandleResult
  {
    [JsonProperty("result")]
    public List<YFCandle> Result { get; set; }
  }

  public class YFCandle
  {
    [JsonProperty("timestamp")]
    public List<long> Timestamp { get; set; }

    [JsonProperty("indicators")]
    public YFQuote Indicators { get; set; }
  }
}
