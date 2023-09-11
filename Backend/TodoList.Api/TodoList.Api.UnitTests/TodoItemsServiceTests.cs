using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TodoList.Api.DTOs;
using TodoList.Api.Models;
using TodoList.Api.Repositories;
using TodoList.Api.Services;
using Xunit;

namespace TodoList.Api.UnitTests
{
    public class TodoItemsServiceTests
    {
        private readonly Mock<ITodoItemsRepository> _todoItemsRepositoryMock;
        private readonly TodoItemsService _todoItemsService;

        public TodoItemsServiceTests()
        {
            _todoItemsRepositoryMock = new Mock<ITodoItemsRepository>();
            _todoItemsService = new TodoItemsService(_todoItemsRepositoryMock.Object);     
        }

        [Fact]
        public async void CreateAsync_Add_Item()
        {
            TodoItemDTO todoItemDto = new TodoItemDTO { Description = "test desc", IsCompleted = false };
            TodoItem todoItem = new TodoItem { Description = "test desc", IsCompleted = false };
            _todoItemsRepositoryMock.Setup(x => x.AddAsync(It.IsAny<TodoItem>(), It.IsAny<CancellationToken>())).ReturnsAsync(todoItem);

            var result = await _todoItemsService.CreateAsync(todoItemDto, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(result.Description, todoItemDto.Description);
            Assert.Equal(result.IsCompleted, todoItemDto.IsCompleted);
        }
        [Fact]
        public async void UpdateAsync_Should_Update_Item()
        {
            TodoItemDTO todoItemDto = new TodoItemDTO { Description = "test desc", IsCompleted = false };
            TodoItem todoItem = new TodoItem { Description = "test desc", IsCompleted = false };
            _todoItemsRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<TodoItem>(), It.IsAny<CancellationToken>()));

            await _todoItemsService.UpdateAsync(Guid.NewGuid(), todoItem, CancellationToken.None);

            _todoItemsRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<TodoItem>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void UpdateAsync_ThrowsException_When_ItemNotExists()
        {
            TodoItem todoItem = new TodoItem { Description = "test desc", IsCompleted = false };
            _todoItemsRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<TodoItem>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Item not found"));

            _ = Assert.ThrowsAsync<Exception>(async () => await _todoItemsService.UpdateAsync(Guid.NewGuid(), todoItem, CancellationToken.None));

            _todoItemsRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<TodoItem>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async void GetAllAsync_Should_Return_Items_When_Exist()
        {
            var todoItems = new List<TodoItem>
            {
                new TodoItem {Id = Guid.NewGuid(), Description = "test desc1", IsCompleted = false },
                new TodoItem {Id = Guid.NewGuid(), Description = "test desc2", IsCompleted = false }
            };
            _todoItemsRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(todoItems);

            var result = await _todoItemsService.GetAllAsync(false, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async void CheckTodoExistsByDescriptionAsync_Should_Return_Items_When_Exist()
        {
            var todoItems = new List<TodoItem>
            {
                new TodoItem {Id = Guid.NewGuid(), Description = "test desc1", IsCompleted = false },
                new TodoItem {Id = Guid.NewGuid(), Description = "test desc2", IsCompleted = false }
            };
            _todoItemsRepositoryMock.Setup(x => x.GetByDescriptionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(todoItems);

            var result = await _todoItemsService.CheckTodoExistsByDescriptionAsync("test desc", CancellationToken.None);

            Assert.True(result);
        }

        [Fact]
        public async void GetByIdAsync_Should_Return_Item_When_Exists()
        {
            TodoItem todoItem = new TodoItem { Id = Guid.NewGuid(), Description = "test desc1", IsCompleted = false };
            _todoItemsRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(todoItem);

            var result = await _todoItemsService.GetByIdAsync(todoItem.Id, CancellationToken.None);

            Assert.NotNull(result);
            Assert.IsType<TodoItemDTO>(result);
            Assert.Equal(result.Id, todoItem.Id);
            Assert.Equal(result.Description, todoItem.Description);
            Assert.Equal(result.IsCompleted, todoItem.IsCompleted);
        }

        [Fact]
        public void GetByIdAsync_Should_Return_Exception_When_Item_Not_Exists()
        {
            TodoItem todoItem = new TodoItem { Id = Guid.NewGuid(), Description = "test desc1", IsCompleted = false };
            _todoItemsRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Item not found"));


            _ = Assert.ThrowsAsync<Exception>(async () => await _todoItemsService.GetByIdAsync(todoItem.Id, CancellationToken.None));
            _todoItemsRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

        }
    }
}
