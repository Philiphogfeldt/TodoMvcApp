using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TodoMvcApp.Interfaces;
using TodoMvcApp.Models;

namespace TodoMvcApp.Data
{
	public class TodoRepository : ITodoRepository
	{
		private readonly AppDbContext _context;

		public TodoRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<TodoItem>> GetAllAsync()
		{
			return await _context.TodoItems.ToListAsync();
		}

		public async Task<TodoItem> GetByIdAsync(int id)
		{
			return await _context.TodoItems.FindAsync(id);
		}

		public async Task AddAsync(TodoItem item)
		{
			_context.TodoItems.Add(item);
			await _context.SaveChangesAsync();

		}

		public async Task UpdateAsync(TodoItem item)
		{
			var existingItem = await _context.TodoItems.FindAsync(item.Id);
			if (existingItem != null)
			{
				existingItem.Name = item.Name;
				existingItem.IsDone = item.IsDone;
				await _context.SaveChangesAsync();
			}
		}

		public async Task DeleteAsync(int id)
		{
			var item = await _context.TodoItems.FindAsync(id);
			if (item != null)
			{
				_context.TodoItems.Remove(item);
				await _context.SaveChangesAsync();
			}
		}

		public async Task ToggleAllAsync()
		{
			var allItems = await _context.TodoItems.ToListAsync();
			bool allCompleted = allItems.All(x => x.IsDone);

			foreach (var item in allItems)
			{
				item.IsDone = !allCompleted;
			}

			await _context.SaveChangesAsync();
		}

		public async Task ClearAllCompletedAsync()
		{
			var completedItems = _context.TodoItems.Where(x => x.IsDone).ToList();
			_context.TodoItems.RemoveRange(completedItems);
			await _context.SaveChangesAsync();
		}
	}
}
