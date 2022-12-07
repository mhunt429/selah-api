using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using Selah.Domain.Data.Models.Plaid;
using Selah.Domain.Internal;
using Selah.Infrastructure.Repository.Interfaces;

namespace Selah.Domain.Data.Repository
{
  public class UserInstitutionRepository: IUserInstitutionRepository
  {
    private readonly IOptions<EnvVariablesConfig> _envVariables;

    public UserInstitutionRepository(IOptions<EnvVariablesConfig> envVariables)
    {
      _envVariables = envVariables;
    }
    public async Task<Guid?> CreateUserInstitution(PlaidUserInstitutionCreate institutionCreate)
    {
      
      using (var connection = new NpgsqlConnection(_envVariables.Value.DbConnectionString))
      {
        return await connection.ExecuteScalarAsync<Guid>(@"INSERT INTO  user_institution (user_id, 
                               institution_id, institution_name, encrypted_access_token, imported_ts)
            VALUES(@user_id, @institution_id,  @institution_name, @encrypted_access_token, @imported_ts)
            RETURNING (id)", new
          {
            user_id = institutionCreate.UserId, 
            institution_id = institutionCreate.InstitutionId,
            institution_name = institutionCreate.InstitutionName,
            encrypted_access_token = institutionCreate.EncryptedAccessToken,
            imported_ts = DateTime.UtcNow
          });
      }
    }

    public async Task<IEnumerable<UserInstitution>> GetByUser(Guid userId)
    {
      using (var connection = new NpgsqlConnection(_envVariables.Value.DbConnectionString))
      {
        return await connection.QueryAsync<UserInstitution>(@"SELECT * FROM 
              user_institution WHERE user_id = @user_id", 
          new {user_id = userId});
      }
    }

    public async Task<UserInstitution> GetById(Guid id)
    {
      using (var connection = new NpgsqlConnection(_envVariables.Value.DbConnectionString))
      {
        return (await connection.QueryAsync<UserInstitution>(@"SELECT * FROM user_institution where id = @id",
          new { id = id })).FirstOrDefault();
      }
    }
  }
}