using Moq;
using Xunit;
using TodoMvcApp.Interfaces;
using TodoMvcApp.Models;
using TodoMvcApp.Controllers;
using TodoMvcApp.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace XUnitTests
{
	public class TodoControllerTests
	{
		[Fact]
		public async Task GetAllTodos_ReturnsActionResultWithTodoItemDtos()
		{
			var mockRepo = new Mock<ITodoRepository>();
			mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<TodoItem>
			{
				new TodoItem { Id = 1, Name = "Test Todo 1", IsComplete = false },
				new TodoItem { Id = 2, Name = "Test Todo 2", IsComplete = true }
			});

			var controller = new TodoController(mockRepo.Object);

			var result = await controller.GetAllTodos();

			Assert.IsType<ActionResult<IEnumerable<TodoItemDto>>>(result);
		}
	}
}
