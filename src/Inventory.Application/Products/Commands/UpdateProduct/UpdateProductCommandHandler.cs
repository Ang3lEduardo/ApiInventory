using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;

namespace Inventory.Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler
    {
        private readonly IProductCommandRepository _repository;

        public UpdateProductCommandHandler(IProductCommandRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task HandleAsync(
            UpdateProductCommand command,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command);

            var product = new Product
            {
                Id = command.Id,
                Name = command.Name,
                CategoryId = command.CategoryId,
                Stock = command.Stock
            };

            await _repository.UpdateAsync(product, cancellationToken);
        }
    }
}