using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;

namespace Inventory.Application.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler
    {
        private readonly ICategoryCommandRepository _repository;

        public UpdateCategoryCommandHandler(ICategoryCommandRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task HandleAsync(
            UpdateCategoryCommand command,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command);

            var category = new Category
            {
                Id = command.Id,
                Name = command.Name
            };

            await _repository.UpdateAsync(category, cancellationToken);
        }
    }
}