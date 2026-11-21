using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Application.Interfaces;

namespace Inventory.Application.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler
    {
        private readonly IProductCommandRepository _repository;

        public DeleteProductCommandHandler(IProductCommandRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task HandleAsync(
            DeleteProductCommand command,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command);

            await _repository.DeleteAsync(command.Id, cancellationToken);
        }
    }
}