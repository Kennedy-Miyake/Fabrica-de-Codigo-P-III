// ReSharper disable All
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
namespace BarCode.Infrastructure.Context;

public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext> {
    public AppDbContext CreateDbContext(string[] args) {
        var connectionString = "Server=database;Port=3306;Database=barcodedb;Uid=appuser;Pwd=appuser@soat1cSu;";
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                             .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                             .Options;
        return new AppDbContext(optionsBuilder);
    }
}