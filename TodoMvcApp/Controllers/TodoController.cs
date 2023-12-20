using Microsoft.AspNetCore.Mvc;
using TodoMvcApp.Interfaces;
using TodoMvcApp.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TodoMvcApp.Controllers
{
	[ApiController]
	[Route("")]
	public class TodoController : ControllerBase
	{
		private readonly ITodoRepository _todoRepository;

		public TodoController(ITodoRepository todoRepository)
		{
			_todoRepository = todoRepository;
		}

		[HttpGet("notes")]
		public async Task<ActionResult<IEnumerable<TodoItem>>> GetAllTodos(bool? isDone = null)
		{
			var todoItems = await _todoRepository.GetAllAsync();

			if (isDone.HasValue)
			{
				todoItems = todoItems.Where(item => item.IsDone == isDone.Value);
			}

			return Ok(todoItems);
		}

		[HttpGet("remaining")]
		public async Task<ActionResult<int>> GetRemainingCount()
		{
			var count = (await _todoRepository.GetAllAsync())
				.Count(item => !item.IsDone);

			return Ok(count);
		}

		[HttpGet("notes/{id}")]
		public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
		{
			var todoItem = await _todoRepository.GetByIdAsync(id);
			if (todoItem == null)
			{
				return NotFound();
			}

			return Ok(todoItem);
		}

		[HttpPost("notes")]
		public async Task<ActionResult<TodoItem>> AddTodoItem(TodoItem todoItem)
		{
			await _todoRepository.AddAsync(todoItem);
			return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
		}


		[HttpPut("notes/{id}")]
		public async Task<IActionResult> UpdateTodoItem(int id, TodoItem todoItem)
		{
			if (id != todoItem.Id)
			{
				return BadRequest();
			}

			var existingItem = await _todoRepository.GetByIdAsync(id);
			if (existingItem == null)
			{
				return NotFound();
			}

			existingItem.Name = todoItem.Name;
			existingItem.IsDone = todoItem.IsDone;

			await _todoRepository.UpdateAsync(existingItem);

			return NoContent();
		}

		[HttpDelete("notes/{id}")]
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

		[HttpPost("clear-completed")]
		public async Task<IActionResult> ClearCompletedTodos()
		{
			await _todoRepository.ClearAllCompletedAsync();
			return NoContent();
		}

	}
}
