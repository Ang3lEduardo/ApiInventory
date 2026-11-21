using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Application.InventoryMovements.Queries.GetInventoryMovementByProductId
{
    public sealed record GetInventoryMovementByProductIdQuery(int ProductId);
}