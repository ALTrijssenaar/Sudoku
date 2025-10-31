using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sudoku.Core.Entities;
using Sudoku.Infrastructure.Data;

namespace Sudoku.Integration.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Override JWT configuration for testing
            var testConfig = new Dictionary<string, string>
            {
                ["Jwt:Secret"] = "test-secret-key-that-is-at-least-32-characters-long",
                ["Jwt:Issuer"] = "SudokuTestApi",
                ["Jwt:Audience"] = "SudokuTestClient",
                ["Jwt:ExpiryMinutes"] = "60"
            };
            config.AddInMemoryCollection(testConfig!);
        });

        builder.ConfigureServices(services =>
        {
            // Remove existing DbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<SudokuDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            
            // Add DbContext using in-memory database for testing
            services.AddDbContext<SudokuDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDatabase");
                options.EnableSensitiveDataLogging();
            });
        });

        builder.UseEnvironment("Testing");
    }
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);
        
        // Seed test data after host is created
        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<SudokuDbContext>();
        SeedTestData(db);
        
        return host;
    }
    
    private static void SeedTestData(SudokuDbContext db)
    {
        // Seed a test user with the hardcoded ID used by SessionsController
        var testUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        
        try
        {
            if (!db.Users.Any(u => u.Id == testUserId))
            {
                var testUser = new User
                {
                    Id = testUserId,
                    Email = "test@example.com",
                    DisplayName = "Test User",
                    PasswordHash = "test-hash",
                    CreatedAt = DateTime.UtcNow
                };
                db.Users.Add(testUser);
                db.SaveChanges();
            }
        }
        catch (ArgumentException)
        {
            // User already exists - this can happen with parallel test execution
            // Ignore the error
        }
        catch (DbUpdateException)
        {
            // User already exists - this can happen with parallel test execution
            // Ignore the error
        }
    }
}