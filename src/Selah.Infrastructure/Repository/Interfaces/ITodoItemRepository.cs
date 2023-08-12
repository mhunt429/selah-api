using Selah.Domain.Data.Models.TodoItem;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Selah.Infrastructure.Repository.Interfaces
{
    public interface ITodoItemRepository
    {
        public Task<int> CreateTodoItem(TodoItem todoCreate);

        public Task<bool> Update(TodoItem todoItem);

        public Task<IReadOnlyList<TodoItem>> GetTodoList(int userId, int limit, int offset);

        public Task<TodoItem> GetTodoByUserAndId(int id, int userId);

        public Task<bool> DeleteTodo(int id);
    }
}
