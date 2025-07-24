// ReSharper disable All
using BarCode.Domain.Models;
using Microsoft.EntityFrameworkCore;
namespace BarCode.Infrastructure.Context;

public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<ProductCompany> ProductCompanies { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
}