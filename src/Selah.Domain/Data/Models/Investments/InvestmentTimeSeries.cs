using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Selah.Domain.Data.Models.Investments
{
  //Request from the web client
  
  //TODO add more parameters 
  //https://developer.tdameritrade.com/price-history/apis/get/marketdata/%7Bsymbol%7D/pricehistory#
  public record TimeSeriesRequest
  {
    [Required]
    public string Ticker { get; set; }
    public string Period { get; set; }

    public string FrequencyType { get; set; } = "daily";
  }
}


// Response Types from TD Ameritrade
public record AssetHistoryResponse
{
  [JsonProperty("candles")]
  public IEnumerable<TimeSeriesFields> Candles { get; set; }
}

public record TimeSeriesFields
{
  [JsonProperty("open")]
  public decimal Open { get; set; }
  
  [JsonProperty("low")]
  public decimal Low { get; set; }
  
  [JsonProperty("close")]
  public decimal Close { get; set; }
  
  [JsonProperty("volume")]
  public long Volume { get; set; }
  
  [JsonProperty("datetime")]
  public long Date { get; set; }

  public DateTime ToDate()
  {
    return DateTimeOffset.FromUnixTimeMilliseconds(this.Date).DateTime;
  }
}

public record TimeSeriesVM
{
  public List<TimeSeries> Series { get; set; }
}

public record TimeSeries
{
  public decimal? Open { get; set; }
  public decimal? Low { get; set; }
  public decimal? Close { get; set; }
  public long? Volume { get; set; }
  public DateTime Date { get; set; }
}