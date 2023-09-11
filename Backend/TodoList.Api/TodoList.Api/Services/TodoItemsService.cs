using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using TodoList.Api.DTOs;
using TodoList.Api.Models;
using TodoList.Api.Repositories;

namespace TodoList.Api.Services
{
    public class TodoItemsService : ITodoItemsService
    {
        private readonly ITodoItemsRepository _todoItemsRepository;
        public TodoItemsService(ITodoItemsRepository todoItemsRepository)
        {
            _todoItemsRepository = todoItemsRepository;
        }
        public async Task<TodoItemDTO> CreateAsync(TodoItemDTO todoItem, CancellationToken cancellationToken)
        {
            var result = await _todoItemsRepository.AddAsync(CreateTodoItemModel(todoItem), cancellationToken);
            return CreateTodoItemDTO(result);
        }

        public async Task<IEnumerable<TodoItemDTO>> GetAllAsync(bool completed, CancellationToken cancellationToken)
        {
            var items = await _todoItemsRepository.GetAllAsync(completed, cancellationToken);
            return items.Select(x =>  new TodoItemDTO { Id = x.Id , Description = x.Description, IsCompleted = x.IsCompleted});
        }

        public async Task<TodoItemDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return CreateTodoItemDTO( await _todoItemsRepository.GetByIdAsync(id, cancellationToken));
        }

        public async Task UpdateAsync(Guid id, TodoItem todoItem, CancellationToken cancellationToken)
        {
            await _todoItemsRepository.UpdateAsync(todoItem, cancellationToken);
            return;
        }
        
        public async Task<bool> CheckTodoExistsByDescriptionAsync(string description, CancellationToken cancellationToken)
        {
            var items = await _todoItemsRepository.GetByDescriptionAsync(description, cancellationToken);
            return items.Any();
        }
        public static TodoItemDTO CreateTodoItemDTO(TodoItem todoItem)
        {
            return new TodoItemDTO { Id = todoItem.Id, Description = todoItem.Description, IsCompleted = todoItem.IsCompleted };
        }
        public static TodoItem  CreateTodoItemModel(TodoItemDTO todoItemDTO)
        {
            return new TodoItem { Id = todoItemDTO.Id, Description = todoItemDTO.Description, IsCompleted = todoItemDTO.IsCompleted };
        }
    }
}
