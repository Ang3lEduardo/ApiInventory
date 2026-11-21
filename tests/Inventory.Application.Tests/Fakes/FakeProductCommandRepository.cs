using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;

namespace Inventory.Application.Tests.Fakes
{
    public class FakeProductCommandRepository : IProductCommandRepository
    {
        private readonly List<Product> _products = [];

        public IReadOnlyList<Product> Products => _products;

        public void Add(Product product)
        {
            _products.Add(product);
        }

        public Task<int> CreateAsync(Product product,
                                     CancellationToken cancellationToken)
        {
            _products.Add(product);

            return Task.FromResult(product.Id);
        }

        public Task UpdateAsync(Product product,
                               CancellationToken cancellationToken)
        {
            if(product.CategoryId <= 0)
            {
                throw new InvalidOperationException(
                    $"A product must have a valid category.");
            }

            var existingProduct = _products
                .FirstOrDefault(x => x.Name == product.Name);

            if (existingProduct is null)
            {
                throw new InvalidOperationException(
                    $"Product with name {product.Name} was not found.");
            }

            existingProduct.Name = product.Name;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.Stock = product.Stock;

            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id,
                                CancellationToken cancellationToken)
        {
            var product = _products
                .FirstOrDefault(x => x.Id == id);

            if (product is null)
            {
                throw new InvalidOperationException(
                    $"Product with id {id} was not found.");
            }

            _products.Remove(product);

            return Task.CompletedTask;
        }
    }
}
