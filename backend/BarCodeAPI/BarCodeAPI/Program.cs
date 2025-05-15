// ReSharper disable all
using System.Text.Json.Serialization;
using BarCode.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace BarCodeAPI;

public class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);
        
        // Recupera valores de ambiente (.env)
        string host = Environment.GetEnvironmentVariable("DB_HOST") ?? throw new InvalidOperationException("DB_HOST is not set");
        string port = Environment.GetEnvironmentVariable("DB_PORT") ?? throw new InvalidOperationException("DB_PORT is not set");
        string database = Environment.GetEnvironmentVariable("DB_NAME") ?? throw new InvalidOperationException("DB_NAME is not set");
        string user = Environment.GetEnvironmentVariable("DB_USER") ?? throw new InvalidOperationException("DB_USER is not set");
        string password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? throw new InvalidOperationException("DB_PASSWORD is not set");
        
        // Monta a connection string
        string connectionString = $"Server={host};Port={port};Database={database};Uid={user};Pwd={password};";
        
        // Registra o DbContext (Pomelo)
        builder.Services.AddDbContext<AppDbContext>(options =>
                                                        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        // Add services to the container.
        builder.Services.AddControllers().AddJsonOptions(options => {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment()) {
            app.MapOpenApi();
            app.MapScalarApiReference(options => {
                List<ScalarServer> servers = new List<ScalarServer>();
                
                string? httpsPort = Environment.GetEnvironmentVariable("ASPNETCORE_HTTPS_PORT");
                if(httpsPort is not null)
                    servers.Add(new ScalarServer($"https://localhost:{httpsPort}"));
                
                string? httpPort = Environment.GetEnvironmentVariable("ASPNETCORE_HTTP_PORT");
                if(httpPort is not null)
                    servers.Add(new ScalarServer($"https://localhost:{httpPort}"));
                
                options.Servers = servers;
                options.Title = "BarCode API";
                options.ShowSidebar = true;
            });
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}