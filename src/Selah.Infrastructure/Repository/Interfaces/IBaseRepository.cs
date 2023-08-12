using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Selah.Infrastructure.Repository.Interfaces
{
    public interface IBaseRepository
    {
        public Task<IEnumerable<T>> GetAllAsync<T>(string sql, object parameters);
        public Task<T> GetFirstOrDefaultAsync<T>(string sql, object parameters);

        public Task<T> AddAsync<T>(string sql, object parameters);

        public Task<bool> UpdateAsync(string sql, object parameters);

        public Task<bool> DeleteAsync(string sql, object parameters);
        
        public Task<int> AddManyAsync<T>(string sql, IReadOnlyCollection<DynamicParameters> objectsToSave);
    }
}
