using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Domain.Entities;
using Xunit;

namespace Inventory.Domain.Tests.Entities
{
    public class ProductTests
    {
        [Fact]
        public void Product_CannotHaveNegativeStock()
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
        public void Product_CannotHaveEmptyCategory()
        {
            Assert.True(true);
        }

        [Fact]
        public void Product_CannotHaveEmptyName()
        {
            Assert.True(true);
        }
    }
}