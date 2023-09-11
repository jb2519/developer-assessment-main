using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using TodoList.Api.DataAccess;
using TodoList.Api.Models;
using TodoList.Api.Repositories;
using Xunit;

namespace TodoList.Api.UnitTests
{
    public class TodoItemRepositoryTests
    {
        private readonly TodoContext _context;
        private readonly TodoItemsRepository _repository;
        public TodoItemRepositoryTests()
        {
            _context = new TodoContext(new DbContextOptionsBuilder<TodoContext>().
                UseInMemoryDatabase(databaseName: "TodoItemInMemoryDB")
                .Options);

            _repository = new TodoItemsRepository(_context);
        }

        [Fact]
        public async void AddAsync_Should_Add_An_Item()
        {
            TodoItem todoItem = new TodoItem { Description = "test desc", IsCompleted = false };

            var result = await _repository.AddAsync(todoItem, CancellationToken.None);

            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.Equal(result.Description, todoItem.Description);
            Assert.Equal(result.IsCompleted, todoItem.IsCompleted);
        }
        [Fact]
        public async void UpdateAsync_Should_Succeed_When_Item_Exists()
        {
            TodoItem todoItem = new TodoItem { Description = "test desc", IsCompleted = false };

            var result = await _repository.AddAsync(todoItem, CancellationToken.None);

            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.Equal(result.Description, todoItem.Description);
            Assert.Equal(result.IsCompleted, todoItem.IsCompleted);
        }
        [Fact]
        public async void GetByIdAsync_Should_Return_Item_When_Exists()
        {
            var itemId = Guid.NewGuid();
            TodoItem todoItem = new TodoItem { Id = itemId, Description = "test desc", IsCompleted = false };
            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();

            var result = await _repository.GetByIdAsync(itemId, CancellationToken.None);
            Assert.NotNull(result);
            Assert.Equal(result.Id, itemId);
        }
        
        [Fact]
        public async void GetAllAsync_Should_Return_List()
        {
            AddTodos();
            var result = await _repository.GetAllAsync(false, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }


        [Fact]
        public async void GetByDescriptionAsync_Should_Return_List()
        {
            AddTodos();
            var result = await _repository.GetByDescriptionAsync("test desc1", CancellationToken.None);
            
            Assert.NotNull(result);
            Assert.Single(result);
        }

        private void AddTodos()
        {
            _context.Database.EnsureDeleted();
            TodoItem todoItem1 = new TodoItem { Description = "test desc1", IsCompleted = false };
            TodoItem todoItem2 = new TodoItem { Description = "test desc2", IsCompleted = false };
            _context.TodoItems.Add(todoItem1);
            _context.TodoItems.Add(todoItem2);
            _context.SaveChanges();
        }
    }
}
