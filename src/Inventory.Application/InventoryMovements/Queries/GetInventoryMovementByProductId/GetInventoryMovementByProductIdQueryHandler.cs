using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Application.DTOs;
using Inventory.Application.Interfaces;

namespace Inventory.Application.InventoryMovements.Queries.GetInventoryMovementByProductId
{
    public class GetInventoryMovementByProductIdQueryHandler
    {
        private readonly IInventoryMovementQueryRepository _inventoryMovementQueryRepository;

        public GetInventoryMovementByProductIdQueryHandler(IInventoryMovementQueryRepository inventoryMovementQueryRepository)
        {
            _inventoryMovementQueryRepository = inventoryMovementQueryRepository;
        }

        public async Task<IEnumerable<InventoryMovementDto>> Handle(GetInventoryMovementByProductIdQuery query, CancellationToken cancellationToken)
        {
            var inventoryMovements = await _inventoryMovementQueryRepository
            .GetByProductIdAsync(query.ProductId, cancellationToken);

            return inventoryMovements;
        }
    }
}