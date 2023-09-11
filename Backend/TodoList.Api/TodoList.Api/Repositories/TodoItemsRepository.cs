using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TodoList.Api.DataAccess;
using TodoList.Api.Models;
using TodoList.Api.Exceptions;

namespace TodoList.Api.Repositories
{
    public class TodoItemsRepository : ITodoItemsRepository
    {
        private readonly TodoContext _context;

        public TodoItemsRepository(TodoContext context)
        {
            _context = context;
        }
        public async Task<TodoItem> AddAsync(TodoItem item, CancellationToken cancellationToken)
        {
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return item;
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync(bool completed, CancellationToken cancellationToken)
        {
            return await _context.TodoItems.Where(x => x.IsCompleted == completed).ToListAsync(cancellationToken);
        }

        public async Task<TodoItem> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var item = await _context.TodoItems.FindAsync(id, cancellationToken);
            if (item == null)
            {
                throw new NotFoundException($"TodoItem with ID {id} not found");
            }
            return item;
        }
        public async Task<IEnumerable<TodoItem>> GetByDescriptionAsync(string description, CancellationToken cancellationToken)
        {
            return await _context.TodoItems.Where(x => x.Description.ToLowerInvariant() == description.ToLowerInvariant() && !x.IsCompleted).ToListAsync(cancellationToken);

        }

        public async Task UpdateAsync(TodoItem item, CancellationToken cancellationToken)
        {
            _context.Entry(item).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemIdExists(item.Id))
                {
                    throw new NotFoundException($"TodoItem with ID {item.Id} not found");
                }
                throw;
            }
        }
        private bool TodoItemIdExists(Guid id)
        {
            return _context.TodoItems.Any(x => x.Id == id);
        }
    }
}
