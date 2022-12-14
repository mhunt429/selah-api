﻿using Dapper;
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
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public BaseRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<T> AddAsync<T>(string sql, object parameters)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                return await connection.ExecuteScalarAsync<T>(sql, parameters);
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
                return await connection.QueryAsync<T>(sql, parameters);
            }
        }

        public async Task<T> GetFirstOrDefaultAsync<T>(string sql, object parameters)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                return (await connection.QueryAsync<T>(sql, parameters)).FirstOrDefault();
            }
        }

        public async Task UpdateAsync(string sql, object parameters)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                await connection.ExecuteAsync(sql, parameters);
                await connection.ExecuteAsync(sql, parameters);
            }
        }
    }
}
