﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selah.Infrastructure.Repository.Interfaces
{
    public interface IBaseRepository
    {
        public Task<IEnumerable<T>> GetAllAsync<T>(string sql, object parameters);
        public Task<T> GetFirstOrDefaultAsync<T>(string sql, object parameters);

        public Task<T> AddAsync<T>(string sql, object parameters);

        public Task UpdateAsync(string sql, object parameters);

        public Task DeleteAsync(string sql, object parameters);

        public Task<int> AddManyAsync(string sql, IReadOnlyCollection<object> objectsToSave);
    }
}
