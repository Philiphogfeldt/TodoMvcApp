using Microsoft.AspNetCore.Mvc;
using TodoMvcApp.Interfaces;
using TodoMvcApp.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using TodoMvcApp.Models;



namespace TodoMvcApp.Controllers
{

	[ApiController]
	[Route("[controller]")]
	public class TodoController : ControllerBase
	{
		private readonly ITodoRepository _todoRepository;

		public TodoController(ITodoRepository todoRepository)
		{
			_todoRepository = todoRepository;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetAllTodos()
		{
			var todoItems = await _todoRepository.GetAllAsync();
			var todoItemDtos = todoItems.Select(item => new TodoItemDto
			{
				Id = item.Id,
				Name = item.Name,
				IsComplete = item.IsComplete
			});

			return Ok(todoItemDtos);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<TodoItemDto>> GetTodoItem(int id)
		{
			var todoItem = await _todoRepository.GetByIdAsync(id);
			if (todoItem == null)
			{
				return NotFound();
			}

			var todoItemDto = new TodoItemDto
			{
				Id = todoItem.Id,
				Name = todoItem.Name,
				IsComplete = todoItem.IsComplete
			};

			return Ok(todoItemDto);
		}

		[HttpPost]
		public async Task<ActionResult<TodoItemDto>> AddTodoItem(TodoItemDto todoItemDto)
		{
			var todoItem = new TodoItem
			{
				Name = todoItemDto.Name,
				IsComplete = todoItemDto.IsComplete
			};

			await _todoRepository.AddAsync(todoItem);

			var newTodoItemDto = new TodoItemDto
			{
				Id = todoItem.Id,
				Name = todoItem.Name,
				IsComplete = todoItem.IsComplete
			};

			return CreatedAtAction(nameof(GetTodoItem), new { id = newTodoItemDto.Id }, newTodoItemDto);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateTodoItem(int id, TodoItemDto todoItemDto)
		{
			if (id != todoItemDto.Id)
			{
				return BadRequest();
			}

			var todoItem = await _todoRepository.GetByIdAsync(id);
			if (todoItem == null)
			{
				return NotFound();
			}

			todoItem.Name = todoItemDto.Name;
			todoItem.IsComplete = todoItemDto.IsComplete;

			await _todoRepository.UpdateAsync(todoItem);

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteTodoItem(int id)
		{
			var todoItem = await _todoRepository.GetByIdAsync(id);
			if (todoItem == null)
			{
				return NotFound();
			}

			await _todoRepository.DeleteAsync(id);

			return NoContent();
		}
	}
}
