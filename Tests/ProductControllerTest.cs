using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseAPI.Controllers;
using WarehouseAPI.Data;
using WarehouseAPI.Models;
using Xunit;

namespace WarehouseAPI.Tests.Controllers;

public class ProductControllerTests
{
    // Helper to generate a fresh, isolated database for each test
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            
        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetProducts_ReturnsAllProducts()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        context.Products.Add(new Product { Id = 1, Name = "Test 1" });
        context.Products.Add(new Product { Id = 2, Name = "Test 2" });
        await context.SaveChangesAsync();
        var controller = new ProductController(context);

        // Act
        var result = await controller.GetProducts();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Product>>>(result);
        Assert.Equal(2, actionResult.Value.Count());
    }

    [Fact]
    public async Task GetProduct_WithValidId_ReturnsProduct()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        context.Products.Add(new Product { Id = 1, Name = "Test 1" });
        await context.SaveChangesAsync();
        var controller = new ProductController(context);

        // Act
        var result = await controller.GetProduct(1);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Product>>(result);
        Assert.Equal(1, actionResult.Value.Id);
    }

    [Fact]
    public async Task GetProduct_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new ProductController(context);

        // Act
        var result = await controller.GetProduct(99);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostProduct_AddsProductAndReturnsCreatedAtAction()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new ProductController(context);
        var newProduct = new Product { Id = 1, Name = "New Product" };

        // Act
        var result = await controller.PostProduct(newProduct);

        // Assert
        var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnedProduct = Assert.IsType<Product>(actionResult.Value);
        Assert.Equal(1, returnedProduct.Id);
        Assert.Equal(1, await context.Products.CountAsync());
    }

    [Fact]
    public async Task UpdateProduct_WithIdMismatch_ReturnsBadRequest()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new ProductController(context);
        var product = new Product { Id = 2, Name = "Mismatch" };

        // Act
        var result = await controller.UpdateProduct(1, product);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("The ID in the URL must match the ID in the body.", badRequestResult.Value);
    }

    [Fact]
    public async Task DeleteProduct_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        context.Products.Add(new Product { Id = 1, Name = "To Delete" });
        await context.SaveChangesAsync();
        var controller = new ProductController(context);

        // Act
        var result = await controller.DeleteProduct(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Equal(0, await context.Products.CountAsync());
    }
}