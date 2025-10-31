using Microsoft.EntityFrameworkCore;
using Sudoku.Core.Repositories;
using Sudoku.Infrastructure.Data;
using Sudoku.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configure database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SudokuDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register repositories
builder.Services.AddScoped<IGameSessionRepository, GameSessionRepository>();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
