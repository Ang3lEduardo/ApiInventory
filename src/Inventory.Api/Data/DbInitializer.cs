using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Data
{
    public static class DbInitializer
{
    public static async Task InitializeAsync(
            InventoryDbContext dbContext)
        {
            await dbContext.Database.MigrateAsync();

            if (!await dbContext.Categories.AnyAsync())
            {
                await SeedDataAsync(dbContext);
            }

            await dbContext.SaveChangesAsync();
        }

        private static async Task SeedDataAsync(
            InventoryDbContext dbContext)
        {
            var electronics = new Category
            {
                Name = "Electronics"
            };

            var furniture = new Category
            {
                Name = "Furniture"
            };

            await dbContext.Categories.AddRangeAsync(
                electronics,
                furniture);

            await dbContext.SaveChangesAsync();

            var products = new List<Product>
            {
                new Product
                {
                    Name = "Laptop",
                    CategoryId = electronics.Id
                },
                new Product
                {
                    Name = "Desk Chair",
                    CategoryId = furniture.Id
                }
            };

            await dbContext.Products.AddRangeAsync(products);
        }
    }
}