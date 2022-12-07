using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Selah.Domain.Data.Models.Integrations;

namespace Selah.Infrastructure.Repository.Interfaces
{
  public interface IAuthorizedAppRepository
  {
    public Task<IEnumerable<UserAuthorizedAppViewModel>> GetAuthorizedApps(Guid userId);
    
    public Task<Guid> SaveOAuthCredentials(UserAuthorizedAppCreate credentials);
    public Task<UserAuthorizedApp> GetOAuthCredentials(string scope, Guid userId);
    
    public Task<int> UpdateOAuthCredentials(UserAuthorizedApp applicationId);
    public Task DeleteOauthCredentials(Guid id);
  }
}