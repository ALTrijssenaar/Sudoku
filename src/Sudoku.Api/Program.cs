using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sudoku.Api.Middleware;
using Sudoku.Core.Repositories;
using Sudoku.Core.Security;
using Sudoku.Core.Services;
using Sudoku.Infrastructure.Data;
using Sudoku.Infrastructure.Repositories;
using Sudoku.Infrastructure.Security;
using Sudoku.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Configure database connection - use InMemory for Testing, PostgreSQL otherwise
if (builder.Environment.IsEnvironment("Testing"))
{
    // In testing environment, the test factory will configure the DbContext
    // Don't register it here to avoid conflicts
}
else
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<SudokuDbContext>(options =>
        options.UseNpgsql(connectionString));
}

// Register repositories
builder.Services.AddScoped<IGameSessionRepository, GameSessionRepository>();
builder.Services.AddScoped<IPuzzleRepository, PuzzleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register services
builder.Services.AddScoped<IPuzzleGenerator, PuzzleGenerator>();
builder.Services.AddScoped<BoardValidator>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<DbInitializer>();

// Configure JWT Authentication
var jwtSecret = builder.Configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret not configured");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "SudokuApi";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "SudokuClient";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
    };
});

builder.Services.AddAuthorization();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// Add Swagger/OpenAPI (Swashbuckle)
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed database in development
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbInitializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
    await dbInitializer.SeedAsync();
}

// Configure the HTTP request pipeline
app.UseCorrelationId();
app.UseErrorHandling();

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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Make Program accessible to tests
public partial class Program { }
