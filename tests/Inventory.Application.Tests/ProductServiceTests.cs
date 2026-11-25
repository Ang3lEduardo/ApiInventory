using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Application.Interfaces;
using Inventory.Application.Products.Commands.CreateProduct;
using Inventory.Application.Products.Commands.UpdateProduct;
using Inventory.Domain.Entities;
using Xunit;

namespace Inventory.Application.Tests
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task CreateProduct_WithValidData_IsCreatedSuccessfully()
        {
            var repository = new FakeProductCommandRepository();
            var handler = new CreateProductCommandHandler(repository);
            using var cancellationSource = new CancellationTokenSource();

            var result = await handler.HandleAsync(
                new CreateProductCommand("Keyboard", 1, 10),
                cancellationSource.Token);

            Assert.Equal(42, result);
            Assert.Equal("Keyboard", repository.CreatedProduct?.Name);
            Assert.Equal(1, repository.CreatedProduct?.CategoryId);
            Assert.Equal(10, repository.CreatedProduct?.Stock);
            Assert.Equal(cancellationSource.Token, repository.CancellationToken);
        }

        [Fact]
        public async Task CreateProduct_WithInvalidCategoryId_ThrowsArgumentException()
        {
            var repository = new FakeProductCommandRepository();
            var handler = new CreateProductCommandHandler(repository);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.HandleAsync(
                    new CreateProductCommand("Keyboard", 0, 10),
                    CancellationToken.None));
        }

        [Fact]
        public async Task CreateProduct_DuplicatedName_ThrowsArgumentException()
        {
            var repository = new FakeProductCommandRepository();
            repository.Add(new Product
            {
                Id = 1,
                Name = "Keyboard",
                CategoryId = 1,
                Stock = 5
            });
            var handler = new CreateProductCommandHandler(repository);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.HandleAsync(
                    new CreateProductCommand("Keyboard", 1, 10),
                    CancellationToken.None));
        }

        [Fact]
        public async Task UpdateProductStock_WithValidQuantity_UpdatesStockSuccessfully()
        {
            var repository = new FakeProductCommandRepository();
            repository.Add(new Product
            {
                Id = 7,
                Name = "Keyboard",
                CategoryId = 1,
                Stock = 10
            });
            var handler = new UpdateProductCommandHandler(repository);

            await handler.HandleAsync(
                new UpdateProductCommand(7, "Keyboard", 1, 25),
                CancellationToken.None);

            Assert.Equal(25, repository.Products[0].Stock);
        }

        [Fact]
        public async Task UpdateProductStock_WithInvalidQuantity_ThrowsArgumentException()
        {
            var repository = new FakeProductCommandRepository();
            repository.Add(new Product
            {
                Id = 7,
                Name = "Keyboard",
                CategoryId = 1,
                Stock = 10
            });
            var handler = new UpdateProductCommandHandler(repository);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.HandleAsync(
                    new UpdateProductCommand(7, "Keyboard", 1, -1),
                    CancellationToken.None));
        }

        private sealed class FakeProductCommandRepository : IProductCommandRepository
        {
            private readonly List<Product> _products = [];

            public IReadOnlyList<Product> Products => _products;
            public Product? CreatedProduct { get; private set; }
            public CancellationToken CancellationToken { get; private set; }

            public void Add(Product product) => _products.Add(product);

            public Task<int> CreateAsync(Product product, CancellationToken cancellationToken)
            {
                if (product.CategoryId <= 0)
                {
                    throw new ArgumentException("The product category is invalid.");
                }

                if (product.Stock < 0)
                {
                    throw new ArgumentException("The stock cannot be negative.");
                }

                if (_products.Any(existing =>
                        string.Equals(existing.Name, product.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ArgumentException("A product with this name already exists.");
                }

                CreatedProduct = product;
                CancellationToken = cancellationToken;
                _products.Add(product);
                return Task.FromResult(42);
            }

            public Task UpdateAsync(Product product, CancellationToken cancellationToken)
            {
                if (product.CategoryId <= 0)
                {
                    throw new ArgumentException("The product category is invalid.");
                }

                if (product.Stock < 0)
                {
                    throw new ArgumentException("The stock cannot be negative.");
                }

                var existingProduct = _products.FirstOrDefault(item => item.Id == product.Id);
                if (existingProduct is null)
                {
                    throw new InvalidOperationException("The product was not found.");
                }

                existingProduct.Name = product.Name;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.Stock = product.Stock;
                return Task.CompletedTask;
            }

            public Task DeleteAsync(int id, CancellationToken cancellationToken) =>
                Task.CompletedTask;
        }
    }
}