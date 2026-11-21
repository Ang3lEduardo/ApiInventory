using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Application.Interfaces;

namespace Inventory.Application.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler
    {
        private readonly ICategoryCommandRepository _repository;

        public DeleteCategoryCommandHandler(ICategoryCommandRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task HandleAsync(
            DeleteCategoryCommand command,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command);

            await _repository.DeleteAsync(command.Id, cancellationToken);
        }
    }
}