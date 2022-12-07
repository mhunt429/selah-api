using Newtonsoft.Json;
using System.Collections.Generic;

namespace Selah.Domain.Data.Models.Integrations.TDAmeritrade
{
    public class TDAmeritradeAccounts
    {
        public SecuritiesAccount SecuritiesAccount { get; set; }
    }
    public class TDAmeritradeAccount
    {
        [JsonProperty("securitiesAccount")]
        public SecuritiesAccount SecuritiesAccount { get; set; }
    }

    public class SecuritiesAccount
    {
        [JsonProperty("accountId")]
        public string AccountId { get; set; }
    }
}
