using Microsoft.EntityFrameworkCore;
using Sudoku.Core.Repositories;
using Sudoku.Core.Services;
using Sudoku.Infrastructure.Data;
using Sudoku.Infrastructure.Repositories;
using Sudoku.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SudokuDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register repositories
builder.Services.AddScoped<IGameSessionRepository, GameSessionRepository>();
builder.Services.AddScoped<IPuzzleRepository, PuzzleRepository>();

// Register services
builder.Services.AddScoped<IPuzzleGenerator, PuzzleGenerator>();
builder.Services.AddScoped<BoardValidator>();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// Add Swagger/OpenAPI (Swashbuckle)
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    // Enable middleware to serve generated Swagger as JSON endpoint and UI in Development
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Sudoku API V1");
        options.RoutePrefix = string.Empty; // serve UI at application root
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Make Program accessible to tests
public partial class Program { }
