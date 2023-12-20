using Moq;
using Xunit;
using TodoMvcApp.Interfaces;
using TodoMvcApp.Models;
using TodoMvcApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace XUnitTests
{
	public class TodoControllerTests
	{
		[Fact]
		public async Task AddTodoItem_ReturnsCreatedTodoItem()
		{
			var mockRepo = new Mock<ITodoRepository>();
			var newItem = new TodoItem
			{
				Name = "New Todo",
				IsDone = false
			};

			mockRepo.Setup(repo => repo.AddAsync(It.IsAny<TodoItem>()))
					.Callback<TodoItem>(item => item.Id = 1)
					.Returns(Task.CompletedTask);

			var controller = new TodoController(mockRepo.Object);
			var result = await controller.AddTodoItem(newItem);

			var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
			var createdTodoItem = Assert.IsType<TodoItem>(actionResult.Value);

			Assert.Equal(1, createdTodoItem.Id);
			Assert.Equal(newItem.Name, createdTodoItem.Name);
			Assert.Equal(newItem.IsDone, createdTodoItem.IsDone);
		}

		[Fact]
		public async Task GetAllTodos_ReturnsTodos()
		{
			var mockRepo = new Mock<ITodoRepository>();
			mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<TodoItem>
			{
			new TodoItem {
			Id = 1,
			Name = "Test Todo 1",
			IsDone = false },

			new TodoItem {
			Id = 2,
			Name = "Test Todo 2",
			IsDone = true }
			});

			var controller = new TodoController(mockRepo.Object);
			var result = await controller.GetAllTodos();

			Assert.IsType<ActionResult<IEnumerable<TodoItem>>>(result);
		}

		[Fact]
		public async Task GetTodoItem_ReturnsTodoItem()
		{
			var mockRepo = new Mock<ITodoRepository>();
			var expectedItem = new TodoItem
			{
				Id = 1,
				Name = "Test Todo",
				IsDone = false
			};

			mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(expectedItem);

			var controller = new TodoController(mockRepo.Object);
			var result = await controller.GetTodoItem(1);

			Assert.IsType<ActionResult<TodoItem>>(result);
			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var returnedItem = Assert.IsType<TodoItem>(okResult.Value);
			Assert.Equal(expectedItem.Id, returnedItem.Id);
			Assert.Equal(expectedItem.Name, returnedItem.Name);
			Assert.Equal(expectedItem.IsDone, returnedItem.IsDone);
		}

		[Fact]
		public async Task GetTodoItem_InvalidId_ReturnsNotFound()
		{
			var mockRepo = new Mock<ITodoRepository>();
			mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((TodoItem)null);

			var controller = new TodoController(mockRepo.Object);
			var result = await controller.GetTodoItem(1);

			Assert.IsType<ActionResult<TodoItem>>(result);
			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async Task DeleteTodoItem_NoContent()
		{
			var mockRepo = new Mock<ITodoRepository>();
			var existingItem = new TodoItem
			{
				Id = 1,
				Name = "Test Todo",
				IsDone = false
			};

			mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingItem);

			var controller = new TodoController(mockRepo.Object);
			var result = await controller.DeleteTodoItem(1);

			Assert.IsType<NoContentResult>(result);
		}

		[Fact]
		public async Task DeleteTodoItem_NotFound()
		{
			var mockRepo = new Mock<ITodoRepository>();
			mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((TodoItem)null);

			var controller = new TodoController(mockRepo.Object);
			var result = await controller.DeleteTodoItem(1);

			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async Task UpdateTodoItem_NoContent()
		{
			var mockRepo = new Mock<ITodoRepository>();
			var updatedItem = new TodoItem
			{
				Id = 1,
				Name = "Updated Todo",
				IsDone = true
			};

			var existingItem = new TodoItem
			{
				Id = 1,
				Name = "Original Todo",
				IsDone = false
			};

			mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingItem);

			var controller = new TodoController(mockRepo.Object);
			var result = await controller.UpdateTodoItem(1, updatedItem);

			Assert.IsType<NoContentResult>(result);
		}

		[Fact]
		public async Task UpdateTodoItem_BadRequest()
		{
			var mockRepo = new Mock<ITodoRepository>();
			var updatedItem = new TodoItem
			{
				Id = 1,
				Name = "Updated Todo",
				IsDone = true
			};

			var controller = new TodoController(mockRepo.Object);
			var result = await controller.UpdateTodoItem(2, updatedItem);

			Assert.IsType<BadRequestResult>(result);
		}

	}
}
