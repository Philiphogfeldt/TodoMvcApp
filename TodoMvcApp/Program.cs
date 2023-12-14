using Microsoft.EntityFrameworkCore;
using TodoMvcApp.Data;
using TodoMvcApp.Interfaces;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the repository
builder.Services.AddScoped<ITodoRepository, TodoRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options =>
	options.AllowAnyOrigin()
		   .AllowAnyMethod()
		   .AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.Run();
