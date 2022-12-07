using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Selah.Application.Services.Interfaces;
using Selah.Domain.Data.Models.Integrations;
using Selah.Infrastructure.Repository.Interfaces;

namespace Selah.Application.Services
{
  public class AuthorizedAppService: IAuthorizedAppService
  {
    private readonly ISecurityService _securityService;
    private readonly IAuthorizedAppRepository _appRepository;
    private ILogger _logger;
    private readonly IHttpService _httpService;
    public AuthorizedAppService(
      ISecurityService securityService, 
      IAuthorizedAppRepository appRepository,
      ILogger<AuthorizedAppService> logger,
      IHttpService httpService
    )
    {
      _securityService = securityService;
      _appRepository = appRepository;
      _logger = logger;
      _httpService = httpService;
    }
    public async Task<Guid> CreateOAuthCredentials(UserAuthorizedAppCreate appToSave)
    {
      _logger.LogInformation($"Creating new OAuth access token for user: {appToSave.UserId}");
      return await _appRepository.SaveOAuthCredentials(appToSave);
    }

    public async Task UpdateOAuthCredentials(UserAuthorizedApp app)
    {
      _logger.LogInformation($"Saving new OAuth access token for user: {app.UserId}");
      await _appRepository.UpdateOAuthCredentials(app);
    }

    public async Task<UserAuthorizedApp> GetOAuthCredentials(string scope, Guid userId)
    {
      return await _appRepository.GetOAuthCredentials(scope, userId);
    }

    public async Task<T> GetOAuthAccessToken<T>(Uri uri, object data, FormUrlEncodedContent content = null)
    {
      var accessTokenResponse = content == null ? await _httpService.PostAsync<T>(data, uri) :
       await _httpService.PostUrlEncodedFormData<T>(uri, content);
      return accessTokenResponse.Item1;
    }

    public async Task<T> RefreshOAuthAccessToken<T>(Uri uri, object data, FormUrlEncodedContent content = null)
    {
      var refreshTokenResponse = content == null ? await _httpService.PostAsync<T>(data, uri) :
        await _httpService.PostUrlEncodedFormData<T>(uri, content);
      return refreshTokenResponse.Item1;
    }
  }
}