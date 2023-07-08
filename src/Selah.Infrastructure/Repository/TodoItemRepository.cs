using Selah.Domain.Data.Models.TodoItem;
using Selah.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selah.Infrastructure.Repository
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly IBaseRepository _baseRepository;

        public TodoItemRepository(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<int> CreateTodoItem(TodoItem todoCreate)
        {
            var sql = @"INSERT INTO todo_item 
                (user_id, recurring, last_completed, frequency, deadline)
                VALUES(@userId, @recurring, @lastCompleted, @frequency, @deadline)
                RETURNING(id)";
            return await _baseRepository.AddAsync<int>(sql, todoCreate);
        }

        public async Task DeleteTodo(int id)
        {
            var sql = "DELETE FROM todo_item WHERE id = @id";

            await _baseRepository.DeleteAsync(sql, new { id });
        }

        public async Task<TodoItem> GetTodoByUserAndId(int id, int userId)
        {
            var sql = "SELECT * FROM todo_item WHERE id = @id AND user_id = @userId";

            return await _baseRepository.GetFirstOrDefaultAsync<TodoItem>(sql, new { id, userId });
        }

        public async Task<IReadOnlyList<TodoItem>> GetTodoList(int userId, int limit = 25, int offset = 0)
        {
            var sql = "SELECT * FROM todo_item WHERE user_id = @userId LIMIT @limit OFFSET @offset";


            return (await _baseRepository.GetAllAsync<TodoItem>(sql, new { userId, limit, offset })).ToList();
        }

        public async Task Update(TodoItem todoItem)
        {
            var sql = @"UPDATE todo_item
            SET    
                recurring = @recurring,
                last_completed = @lastCompleted,
                frequency = @frequency,
                deadline = @deadline
            WHERE  id = @id ";

            var parameters = new { 
                recurring = todoItem.Recurring, 
                lastCompleted = todoItem.LastCompleted, 
                frequency = todoItem.Frequency,
                deadline = todoItem.Deadline 
            };

            await _baseRepository.UpdateAsync(sql, parameters);
        }
    }
}
