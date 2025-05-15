// ReSharper disable All
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
namespace BarCode.Infrastructure.Context;

public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext> {
    public AppDbContext CreateDbContext(string[] args) {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddUserSecrets<AppDbContextFactory>(optional: true)
            .Build();
        var connectionString = configuration["DB_CONNECTION"];
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                             .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                             .Options;
        return new AppDbContext(optionsBuilder);
    }
}