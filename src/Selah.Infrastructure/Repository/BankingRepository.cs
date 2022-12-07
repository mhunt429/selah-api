using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using Selah.Domain.Data.Models.Banking;
using Selah.Domain.Internal;
using Selah.Infrastructure.Repository.Interfaces;

namespace Selah.Infrastructure.Repository
{
  public class BankingRepository: IBankingRepository
  {
    private readonly IOptions<EnvVariablesConfig> _envVariables;

    public BankingRepository(IOptions<EnvVariablesConfig> envVariables)
    {
      _envVariables = envVariables;
    }

    public async Task<IEnumerable<BankAccount>> GetAccounts(Guid userId)
    {
      using (var connection = new NpgsqlConnection(_envVariables.Value.DbConnectionString))
      {
        var accounts = await connection.QueryAsync<BankAccount>(@"SELECT 
                    id, 
                    external_account_id, 
                    account_mask,  
                    account_name, 
                    available_balance,
                    current_balance,
                    user_id,
                    subtype,
                    institution_id 
                    FROM user_bank_account
                    WHERE user_id = @user_id
                    ORDER BY account_name", new{user_id = userId});
        return accounts;
      }
    }
    
    public async Task<IEnumerable<BankAccount>> GetAccountsByInstitutionId(Guid institutionId)
    {
      using (var connection = new NpgsqlConnection(_envVariables.Value.DbConnectionString))
      {
        var accounts = await connection.QueryAsync<BankAccount>(@"SELECT 
                    id, 
                    external_account_id, 
                    account_mask,  
                    account_name, 
                    available_balance,
                    current_balance,
                    user_id,
                    subtype,
                    institution_id 
                    FROM user_bank_account
                    WHERE institution_id = @institution_id", 
          new {institution_id = institutionId});
        return accounts;
      }
    }

    public async Task<long> ImportAccounts(List<BankAccount> accounts)
    {
      using (var connection = new NpgsqlConnection(_envVariables.Value.DbConnectionString))
      {
        var insertedRows = 0;

        foreach (var account in accounts )
        {
          var row = await connection.ExecuteAsync(@"INSERT INTO user_bank_account(
                    external_account_id, 
                    account_mask, 
                    account_name, 
                    available_balance,
                    current_balance,
                    user_id,
                    subtype,
                    institution_id) values(
                               @external_account_id, 
                    @account_mask, 
                    @account_name, 
                    @available_balance,
                    @current_balance,
                    @user_id,
                    @subtype,
                    @institution_id             
                    )", new
          {
            external_account_id = account.ExternalAccountId,
            account_mask = account.Mask,
            account_name = account.Name,
            available_balance = account.AvailableBalance,
            current_balance = account.CurrentBalance,
            user_id = account.UserId,
            subtype = account.Subtype,
            institution_id = account.InstitutionId
          });
          insertedRows += row;
        }

        return insertedRows;
      }
    }
  }
}