using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using Selah.Domain.Internal;
using Selah.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Selah.Infrastructure.Repository
{
    public class BaseRepository : IBaseRepository
    {
        private readonly IOptions<EnvVariablesConfig> _envVariables;


        public BaseRepository(IOptions<EnvVariablesConfig> envVariables)
        {
            _envVariables = envVariables;
        }

        public async Task<T> AddAsync<T>(string sql, object parameters)
        {
            using (var connection = new NpgsqlConnection(_envVariables.Value.DbConnectionString))
            {
                await connection.OpenAsync();
                return await connection.ExecuteScalarAsync<T>(sql, parameters);
            }
        }

        public async Task DeleteAsync(string sql, object parameters)
        {
            using (var connection = new NpgsqlConnection(_envVariables.Value.DbConnectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(sql, parameters);
            }
        }
        public async Task<IEnumerable<T>> GetAllAsync<T>(string sql, object parameters)
        {
            using (var connection = new NpgsqlConnection(_envVariables.Value.DbConnectionString))
            {
                await connection.OpenAsync();
               return await connection.QueryAsync<T>(sql, parameters);
            }
        }

        public async Task<T> GetFirstOrDefaultAsync<T>(string sql, object parameters)
        {
            using (var connection = new NpgsqlConnection(_envVariables.Value.DbConnectionString))
            {
                await connection.OpenAsync();
                return (await connection.QueryAsync<T>(sql, parameters)).FirstOrDefault();
            }
        }

        public async Task UpdateAsync(string sql, object parameters)
        {
            using (var connection = new NpgsqlConnection(_envVariables.Value.DbConnectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(sql, parameters);
            }
        }
    }
}
