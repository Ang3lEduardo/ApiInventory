using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;

namespace Inventory.Application.Products.Queries.GetProducts
{
    public class GetProductsQueryHandler
    {
        private readonly IProductQueryRepository _productQueryRepository;

        public GetProductsQueryHandler(IProductQueryRepository productQueryRepository)
        {
            _productQueryRepository = productQueryRepository;
        }

        public async Task<List<Product>> Handle(GetProductsQuery query, 
                                    CancellationToken cancellationToken)
        {
            return await _productQueryRepository.GetAllAsync(cancellationToken);
        }
    }
}