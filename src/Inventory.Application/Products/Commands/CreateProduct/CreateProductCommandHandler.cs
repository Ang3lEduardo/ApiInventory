using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;

namespace Inventory.Application.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler
    {
        private readonly IProductCommandRepository _repository;

        public CreateProductCommandHandler(IProductCommandRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<int> HandleAsync(
            CreateProductCommand command,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command);

            var product = Product.Create(
                            command.Name,
                            command.CategoryId,
                            command.Stock);

            return await _repository.CreateAsync(product, cancellationToken);
        }
    }
}