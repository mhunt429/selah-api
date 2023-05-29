using Dapper;
using Microsoft.Extensions.Logging;
using Selah.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Selah.Infrastructure.Repository
{
    public class BaseRepository : IBaseRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private ILogger<BaseRepository> _logger;
        public BaseRepository(IDbConnectionFactory dbConnectionFactory, ILogger<BaseRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        public async Task<T> AddAsync<T>(string sql, object parameters)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                try
                {
                    return await connection.ExecuteScalarAsync<T>(sql, parameters);

                }
                catch (Exception ex)
                {
                    _logger.LogError($"Add async failed with error {ex.StackTrace}");
                    return default(T);
                }
            }
        }

        public async Task<int> AddManyAsync<T>(string sql, IReadOnlyCollection<DynamicParameters> objectsToSave)
        {
            int rowsInserted = 0;
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                try
                {
                    foreach (var obj in objectsToSave)
                    {
                        await connection.ExecuteScalarAsync(sql, obj);
                        rowsInserted++;
                    }
                    return rowsInserted;

                }
                catch (Exception ex)
                {
                    _logger.LogError($"Add async failed with error {ex.StackTrace}");
                    return 0;
                }
            }
        }

        public async Task DeleteAsync(string sql, object parameters)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                await connection.ExecuteAsync(sql, parameters);
            }
        }
        public async Task<IEnumerable<T>> GetAllAsync<T>(string sql, object parameters)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                DefaultTypeMap.MatchNamesWithUnderscores = true;
                return await connection.QueryAsync<T>(sql, parameters);
            }
        }

        public async Task<T> GetFirstOrDefaultAsync<T>(string sql, object parameters)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                DefaultTypeMap.MatchNamesWithUnderscores = true;
                return (await connection.QueryAsync<T>(sql, parameters)).FirstOrDefault();
            }
        }

        public async Task UpdateAsync(string sql, object parameters)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                await connection.ExecuteAsync(sql, parameters);
            }
        }
    }
}
