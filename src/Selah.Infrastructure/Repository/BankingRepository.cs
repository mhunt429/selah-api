using System.Collections.Generic;
using System.Threading.Tasks;
using Selah.Domain.Data.Models.Banking;
using Selah.Infrastructure.Repository.Interfaces;

namespace Selah.Infrastructure.Repository
{
    public class BankingRepository : IBankingRepository
    {
        private readonly IBaseRepository _baseRepository;

        public BankingRepository(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<IEnumerable<BankAccount>> GetAccounts(int userId, int limit = 25, int offset = 1)
        {
            string sql = @"SELECT 
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
                    ORDER BY account_name
                    LIMIT @limit OFFSET @offset";

            var parameters = new
            {
                user_id = userId, limit, offset
            };

            return await _baseRepository.GetAllAsync<BankAccount>(sql, parameters);
        }

        public async Task<IEnumerable<BankAccount>> GetAccountsByInstitutionId(int institutionId)
        {
            string sql = @"SELECT 
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
                    WHERE institution_id = @institution_id";

            var parameters = new
            {
                institution_id = institutionId
            };
            return await _baseRepository.GetAllAsync<BankAccount>(sql, parameters);
        }

        public async Task<BankAccount> GetAccountById(int id)
        {
            string sql = @"SELECT 
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
                    WHERE id = @id";
            return await _baseRepository.GetFirstOrDefaultAsync<BankAccount>(sql, new { id });
        }
    }
}