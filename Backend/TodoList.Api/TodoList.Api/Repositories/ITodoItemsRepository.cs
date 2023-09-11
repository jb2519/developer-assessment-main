using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TodoList.Api.DTOs;
using TodoList.Api.Models;

namespace TodoList.Api.Repositories
{
    public interface ITodoItemsRepository
    {
        Task<IEnumerable<TodoItem>> GetAllAsync(bool completed, CancellationToken cancellationToken);
        Task<TodoItem> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<IEnumerable<TodoItem>> GetByDescriptionAsync(string description, CancellationToken cancellationToken);
        Task<TodoItem> AddAsync(TodoItem item, CancellationToken cancellationToken);
        Task UpdateAsync(TodoItem item, CancellationToken cancellationToken);
        //Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
