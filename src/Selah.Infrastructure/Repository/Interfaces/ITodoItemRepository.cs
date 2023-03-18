using Selah.Domain.Data.Models.TodoItem;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Selah.Infrastructure.Repository.Interfaces
{
    public interface ITodoItemRepository
    {
        public Task<long> CreateTodoItem(TodoItem todoCreate);

        public Task Update(TodoItem todoItem);

        public Task<IReadOnlyList<TodoItem>> GetTodoList(Guid userId, int limit, int offset);

        public Task<TodoItem> GetTodoByUserAndId(long id, Guid userId);

        public Task DeleteTodo(long id);
    }
}
