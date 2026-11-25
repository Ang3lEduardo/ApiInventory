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
                throw new ArgumentException(
                    $"El producto debe tener una categoría válida.");
            }

            var existingProduct = _products
                .FirstOrDefault(x => x.Id == product.Id)
                ?? _products.FirstOrDefault(x =>
                    string.Equals(x.Name, product.Name, StringComparison.OrdinalIgnoreCase));

            if (existingProduct is null)
            {
                throw new InvalidOperationException(
                    $"El producto con nombre {product.Name} no fue encontrado.");
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
                    $"El producto con id {id} no fue encontrado.");
            }

            _products.Remove(product);

            return Task.CompletedTask;
        }
    }
}
