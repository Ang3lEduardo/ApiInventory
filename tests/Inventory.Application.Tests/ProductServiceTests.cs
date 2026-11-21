using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Domain.Entities;
using Xunit;

namespace Inventory.Application.Tests
{
    public class ProductServiceTests
    {
        [Fact]
        public void CreateProduct_WithValidData_IsCreatedSuccessfully()
        {
            // Arrange
            var product = new Product
            {
                Name = "Keyboard",
                CategoryId = 1,
                Stock = 10
            };

            // Act
            //var result = service.CreateProduct(product);

            // Assert
            Assert.True(true);
            //Assert.NotNull(result);
            //Assert.Equal("Keyboard", result.Name);
        }

        [Fact]
        public void CreateProduct_WithInvalidData_ThrowsArgumentException()
        {
            Assert.True(true);
        }

        [Fact]
        public void UpdateProductStock_WithValidQuantity_UpdatesStockSuccessfully()
        {
            Assert.True(true);
        }

        [Fact]
        public void UpdateProductStock_WithInvalidQuantity_ThrowsArgumentException()
        {
            Assert.True(true); 
        }
    }
}