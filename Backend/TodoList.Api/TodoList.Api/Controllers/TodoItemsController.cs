using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TodoList.Api.DTOs;
using TodoList.Api.Models;
using TodoList.Api.Exceptions;
using TodoList.Api.Services;

namespace TodoList.Api.Controllers
{
    [ApiController]
    [Route("v1/api/[controller]")]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemsService _todoItemsService;
        private readonly ILogger<TodoItemsController> _logger;

        public TodoItemsController(ITodoItemsService todoItemsService, ILogger<TodoItemsController> logger)
        {
            _todoItemsService = todoItemsService;
            _logger = logger;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<IActionResult> GetTodoItemsPending(CancellationToken cancellationToken)
        {
            var results = await _todoItemsService.GetAllAsync(false, cancellationToken);
            return Ok(results);
        }

        // GET: api/TodoItems/...
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoItem(Guid id, CancellationToken cancellationToken)
        {
            var result = await _todoItemsService.GetByIdAsync(id, cancellationToken);

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


        // PUT: api/TodoItems/... 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(Guid id, TodoItem todoItem, CancellationToken cancellationToken)
        {
            try
            {
                if (id != todoItem.Id)
                {
                    return BadRequest();
                }
                await _todoItemsService.UpdateAsync(id, todoItem, cancellationToken);
            }
            catch (Exception ex ) when (ex is NotFoundException)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

        // POST: api/TodoItems 
        [HttpPost]
        public async Task<IActionResult> PostTodoItem(TodoItemDTO todoItem, CancellationToken cancellationToken)
        {
            if (await _todoItemsService.CheckTodoExistsByDescriptionAsync(todoItem.Description, cancellationToken))
            {
                return BadRequest(TodoItemsConstants.DuplicateItemErrorMessage);
            }
            var result = await _todoItemsService.CreateAsync(todoItem, cancellationToken);
            return CreatedAtAction(nameof(GetTodoItem), new { id = result.Id }, result);
        }
    }
}
