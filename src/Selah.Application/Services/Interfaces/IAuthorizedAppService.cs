using System;
using System.Net.Http;
using System.Threading.Tasks;
using Selah.Domain.Data.Models.Integrations;
using Selah.Domain.Data.Models.Integrations.TDAmeritrade;

namespace Selah.Application.Services.Interfaces
{
  public interface IAuthorizedAppService
  {
    public Task<Guid> CreateOAuthCredentials(UserAuthorizedAppCreate appToSave);
    public Task UpdateOAuthCredentials(UserAuthorizedApp app);
    public Task<UserAuthorizedApp> GetOAuthCredentials(string scope, Guid userId);

      /// <summary>
      /// Used to get or refresh a new combination of access tokens and refresh tokens any 3rd Party integration we support
      /// </summary>
      /// <param name="uri"></param>
      /// <param name="data"></param>
      /// <param name="content"></param>
      /// <typeparam name="T"></typeparam>
      /// <typeparam name="U"></typeparam>
      /// <returns></returns>
    public Task<T> GetOAuthAccessToken<T>(Uri uri, object data, FormUrlEncodedContent content = null);
    public Task<T> RefreshOAuthAccessToken<T>(Uri uri, object data, FormUrlEncodedContent content = null);
  }
}