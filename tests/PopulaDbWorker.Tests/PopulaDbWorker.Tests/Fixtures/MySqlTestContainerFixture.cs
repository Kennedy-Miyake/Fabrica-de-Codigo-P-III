using BarCode.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MySql;

namespace PopulaDbWorker.Tests.Fixtures;

public class MySqlTestContainerFixture : IAsyncLifetime {
    public MySqlContainer MySqlContainer { get; private set; } = null!;
    public DbContextOptions<AppDbContext> DbContextOptions { get; private set; } = null!;

    public async Task InitializeAsync() {
        MySqlContainer = new MySqlBuilder()
                         .WithDatabase("testdb")
                         .WithUsername("test")
                         .WithPassword("test")
                         .WithImage("mysql:8.0")
                         .Build();
        await MySqlContainer.StartAsync();
        
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseMySql(MySqlContainer.GetConnectionString(), ServerVersion.AutoDetect(MySqlContainer.GetConnectionString()));
        DbContextOptions = optionsBuilder.Options;
        
        using var context = new AppDbContext(DbContextOptions);
        await context.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync() {
        await MySqlContainer.StopAsync();
    }
}