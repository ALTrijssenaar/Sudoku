using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sudoku.Infrastructure.Data;

namespace Sudoku.Integration.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Override configuration to prevent loading PostgreSQL connection string
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = "InMemory"
            });
        });

        builder.ConfigureServices(services =>
        {
            // Remove all DbContext-related registrations
            services.RemoveAll(typeof(DbContextOptions<SudokuDbContext>));
            services.RemoveAll(typeof(DbContextOptions));
            services.RemoveAll(typeof(SudokuDbContext));

            // Add DbContext using in-memory database for testing
            services.AddDbContext<SudokuDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryTestDb_" + Guid.NewGuid());
            });
        });

        builder.UseEnvironment("Testing");
    }
}
