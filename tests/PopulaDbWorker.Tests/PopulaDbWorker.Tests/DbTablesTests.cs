// ReSharper disable All
using BarCode.Domain.Models;
using BarCode.Infrastructure.Context;
using PopulaDbWorker.Models;
using System.Text.Json;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PopulaDbWorker.Tests.Fixtures;

namespace PopulaDbWorker.Tests;

public class DbTablesTests : IClassFixture<MySqlTestContainerFixture> {
    private const string SampleJson =
        """
        {
            "name": "Sample Product",
            "description": "This is a sample product.",
            "image_url": "http://example.com/sample.jpg",
            "barcode": "1234567890123"
        }
        """;
    
    private readonly DbContextOptions<AppDbContext> _dbContextOptions;
    
    public DbTablesTests(MySqlTestContainerFixture fixture) {
        _dbContextOptions = fixture.DbContextOptions;
    }

    [Fact]
    public void Fill_DbTables_With_Json_Should_Work() {
        // Arrange
        var dbTables = new DbTables();
        using var document = JsonDocument.Parse(SampleJson);
        var root = document.RootElement;
        
        // Act
        dbTables.SetProductAttribute(ProductAttributes.NAME, root.GetProperty("name").ToString());
        dbTables.SetProductAttribute(ProductAttributes.DESCRIPTION, root.GetProperty("description").ToString());
        dbTables.SetProductAttribute(ProductAttributes.IMAGE_URL, root.GetProperty("image_url").ToString());
        dbTables.SetProductAttribute(ProductAttributes.BARCODE, root.GetProperty("barcode").ToString());
        
        // Assert
        dbTables.GetProductAttribute(ProductAttributes.NAME).Should().Be("Sample Product");
        dbTables.GetProductAttribute(ProductAttributes.DESCRIPTION).Should().Be("This is a sample product.");
        dbTables.GetProductAttribute(ProductAttributes.IMAGE_URL).Should().Be("http://example.com/sample.jpg");
        dbTables.GetProductAttribute(ProductAttributes.BARCODE).Should().Be("1234567890123");
    }

    [Fact]
    public void Insertion_Into_The_Database_Should_Work() {
        // Arrange I
        var dbTables = new DbTables();
        using var document = JsonDocument.Parse(SampleJson);
        var root = document.RootElement;
        
        dbTables.SetProductAttribute(ProductAttributes.NAME, root.GetProperty("name").ToString());
        dbTables.SetProductAttribute(ProductAttributes.DESCRIPTION, root.GetProperty("description").ToString());
        dbTables.SetProductAttribute(ProductAttributes.IMAGE_URL, root.GetProperty("image_url").ToString());
        dbTables.SetProductAttribute(ProductAttributes.BARCODE, root.GetProperty("barcode").ToString());
        
        // Arrange II
        var product = new Product {
            Name = dbTables.GetProductAttribute(ProductAttributes.NAME)!.ToString(),
            Description = dbTables.GetProductAttribute(ProductAttributes.DESCRIPTION)!.ToString(),
            ImageUrl = dbTables.GetProductAttribute(ProductAttributes.IMAGE_URL)!.ToString(),
            BarCode = dbTables.GetProductAttribute(ProductAttributes.BARCODE)!.ToString()
        };
    }
}