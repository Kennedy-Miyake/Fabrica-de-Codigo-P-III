// ReSharper disable all

using BarCodeAPI.Context;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace BarCodeAPI;

public class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);
        string connectionString = $"Server={host};Port={port};Database={database};Uid={user};Pwd={password};";

        // Add services to the container.
        builder.Services.AddControllers();
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