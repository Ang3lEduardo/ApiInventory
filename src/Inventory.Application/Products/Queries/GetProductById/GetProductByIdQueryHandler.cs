using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;

namespace Inventory.Application.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler
    {
        private readonly IProductQueryRepository _repository;

        public GetProductByIdQueryHandler(IProductQueryRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Product?> HandleAsync(
            GetProductByIdQuery query,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(query);

            return await _repository.GetByIdAsync(query.Id, cancellationToken);
        }
    }
}