using TodoList.Api.Controllers;
using Xunit;
using Moq;
using TodoList.Api.Services;
using Microsoft.Extensions.Logging;
using System;
using TodoList.Api.DTOs;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using TodoList.Api.Exceptions;
using TodoList.Api.Models;

namespace TodoList.Api.UnitTests
{
    public class TodoItemsControllerTests
    {
        private readonly TodoItemsController _todoItemsController;
        private readonly Mock<ITodoItemsService> _todoItemsServiceMock;
        private readonly Mock<ILogger<TodoItemsController>> _loggerMock;

        public TodoItemsControllerTests()
        {
            _todoItemsServiceMock = new Mock<ITodoItemsService>();
            _loggerMock = new Mock<ILogger<TodoItemsController>>();
            _todoItemsController = new TodoItemsController(_todoItemsServiceMock.Object, _loggerMock.Object);
        }
        [Fact]
        public async void GetToDoAsync_Success()
        {
            _todoItemsServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new TodoItemDTO { });
            var result = await _todoItemsController.GetTodoItem(Guid.NewGuid(), CancellationToken.None);
            
            Assert.NotNull(result);  
            _todoItemsServiceMock.Verify(x=> x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void PutTodoItem_Throws_Exception()
        {
            var id = Guid.NewGuid();
            _todoItemsServiceMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<TodoItem>(), It.IsAny<CancellationToken>())).ThrowsAsync(new NotFoundException("item not found"));

            _ = Assert.ThrowsAsync<Exception>(async () => await _todoItemsController.PutTodoItem(id, new TodoItem { Id= id }, CancellationToken.None));
            _todoItemsServiceMock.Verify(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<TodoItem>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
