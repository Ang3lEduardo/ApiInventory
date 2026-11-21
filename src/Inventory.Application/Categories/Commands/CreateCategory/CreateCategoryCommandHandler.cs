using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;

namespace Inventory.Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler
    {
        private readonly ICategoryCommandRepository _repository;

        public CreateCategoryCommandHandler(ICategoryCommandRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<int> HandleAsync(
            CreateCategoryCommand command,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command);

            var category = new Category
            {
                Name = command.Name
            };

            return await _repository.CreateAsync(category, cancellationToken);
        }
    }
}