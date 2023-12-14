using Microsoft.EntityFrameworkCore;
using TodoMvcApp.Models;

namespace TodoMvcApp.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<TodoItem> TodoItems { get; set; }
		
	}

}
