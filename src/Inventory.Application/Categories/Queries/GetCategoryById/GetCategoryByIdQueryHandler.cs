using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;

namespace Inventory.Application.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryHandler
    {
        private readonly ICategoryQueryRepository _repository;

        public GetCategoryByIdQueryHandler(ICategoryQueryRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Category?> HandleAsync(
            GetCategoryByIdQuery query,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(query);

            return await _repository.GetByIdAsync(query.Id, cancellationToken);
        }
    }
}