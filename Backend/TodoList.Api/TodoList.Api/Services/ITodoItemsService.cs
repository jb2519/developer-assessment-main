using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TodoList.Api.DTOs;
using TodoList.Api.Models;

namespace TodoList.Api.Services
{
    public interface ITodoItemsService
    {
        public Task<IEnumerable<TodoItemDTO>> GetAllAsync(bool completed, CancellationToken cancellationToken);
        public Task<TodoItemDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken); 
        public Task<bool> CheckTodoExistsByDescriptionAsync(string description, CancellationToken cancellationToken);
        public Task UpdateAsync(Guid id, TodoItem todoItem, CancellationToken cancellationToken);
        public Task<TodoItemDTO> CreateAsync(TodoItemDTO todoItem, CancellationToken cancellationToken);
    }
}
