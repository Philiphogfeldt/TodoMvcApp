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

	}
}
