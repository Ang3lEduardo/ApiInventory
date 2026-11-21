using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;

namespace Inventory.Application.Categories.Queries.GetCategories
{
    public class GetCategoriesQueryHandler
    {
        private readonly ICategoryQueryRepository _repository;

        public GetCategoriesQueryHandler(ICategoryQueryRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<Category>> HandleAsync(
            GetCategoriesQuery query,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(query);

            return await _repository.GetAllAsync(cancellationToken);
        }
    }
}