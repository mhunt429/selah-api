using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using Selah.Domain.Data.Models.Integrations;
using Selah.Domain.Internal;
using Selah.Infrastructure.Repository.Interfaces;

namespace Selah.Infrastructure.Repository
{
  public class AuthorizedAppRepository: IAuthorizedAppRepository
  {
    private readonly IOptions<EnvVariablesConfig> _envVariables;

    public AuthorizedAppRepository(IOptions<EnvVariablesConfig> envVariables)
    {
      _envVariables = envVariables;
    }
    public async Task<IEnumerable<UserAuthorizedAppViewModel>> GetAuthorizedApps(Guid userId)
    {
      using (var connection = new NpgsqlConnection(_envVariables.Value.DbConnectionString))
      {
        var apps = await connection.QueryAsync<UserAuthorizedAppViewModel>(@"SELECT id, 
       scope, created_ts, updated_ts FROM user_authorized_app WHERE user_id = @user_id", new { user_id = userId });
        return apps;
      }
    }

    public async Task<Guid> SaveOAuthCredentials(UserAuthorizedAppCreate credentials)
    {
      using (var connection = new NpgsqlConnection(_envVariables.Value.DbConnectionString))
      {
        var insertedId = await connection.ExecuteScalarAsync<Guid>(@"INSERT INTO user_authorized_app
            (user_id, scope, encrypted_access_token, encrypted_refresh_token, 
             access_token_expiration_ts, refresh_token_expiration_ts, created_ts, updated_ts)
             values(@user_id, @scope, @encrypted_access_token, @encrypted_refresh_token, 
             @access_token_expiration_ts, @refresh_token_expiration_ts, @created_ts, @updated_ts) RETURNING(id)", new
        {
          user_id = credentials.UserId,
          scope = credentials.Scope,
          encrypted_access_token = credentials.EncryptedAccessToken,
          encrypted_refresh_token = credentials.EncryptedRefreshToken,
          access_token_expiration_ts = credentials.AccessTokenExpirationTs,
          refresh_token_expiration_ts = credentials.RefreshTokenExpirationTs,
          created_ts = credentials.CreatedTs,
          updated_ts = credentials.UpdatedTs 
        });
        return insertedId;
      }
    }

    public async Task<UserAuthorizedApp> GetOAuthCredentials(string scope, Guid userId)
    {
      using (var connection = new NpgsqlConnection(_envVariables.Value.DbConnectionString))
      {
        var credentials =
          await connection.QueryAsync<UserAuthorizedApp>(@"SELECT * FROM user_authorized_app WHERE user_id = @user_id AND 
            scope = @scope",
            new { user_id = userId, scope });
        return credentials.FirstOrDefault();
      }
    }

    public async Task DeleteOauthCredentials(Guid id)
    {
      using (var connection = new NpgsqlConnection(_envVariables.Value.DbConnectionString))
      {
        await connection.ExecuteAsync(@"DELETE FROM user_authorized_app where id = @id", new { id });
      }
    }

    public async Task<int> UpdateOAuthCredentials(UserAuthorizedApp app)
    {
      using (var connection = new NpgsqlConnection(_envVariables.Value.DbConnectionString))
      {
        var updatedRows = await connection.ExecuteAsync(@"UPDATE user_authorized_app SET 
            encrypted_access_token = @encrypted_access_token,
            encrypted_refresh_token = @encrypted_refresh_token,
            access_token_expiration_ts = @access_token_expiration_ts,
            refresh_token_expiration_ts = @refresh_token_expiration_ts,
            updated_ts = @updated_ts", new
        {
          encrypted_access_token = app.EncryptedAccessToken,
          encrypted_refresh_token = app.EncryptedRefreshToken,
          access_token_expiration_ts = app.AccessTokenExpirationTs,
          refresh_token_expiration_ts = app.RefreshTokenExpirationTs,
          updated_ts = app.UpdatedTs
        });
        return updatedRows;
      }
    }
  }
}