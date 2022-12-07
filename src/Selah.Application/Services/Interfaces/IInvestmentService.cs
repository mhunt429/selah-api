using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Selah.Domain.Data.Models.Integrations.TDAmeritrade;
using Selah.Domain.Data.Models.Investments;

namespace Selah.Application.Services.Interfaces
{
    public interface IInvestmentService
    {
        public Task<TimeSeriesVM> GetHistoryByTicker(TimeSeriesRequest request, Guid userId);

        //TODO Move these to a possible generic integration interface

        public string GetTdAmeritradeAuthUrl();

        public Task<OAuthTokenResponse> GetTdAmeritradeAuthToken(string authCode);
        public Task<OAuthTokenResponse> RefreshTdAmeritradeAuthToken(string refreshToken);
        public Task<List<TDAmeritradeAccounts>> GetTdAmeritradeAccounts(Guid userId);

    }
}