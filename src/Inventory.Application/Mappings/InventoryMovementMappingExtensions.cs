using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Application.DTOs;

namespace Inventory.Application.Mappings
{
    public static class InventoryMovementMappingExtensions
    {
        public static InventoryMovementDto ToDto(this InventoryMovementDto movement)
        {
            return new InventoryMovementDto
            {
                ProductName = movement.ProductName,
                MovementType = movement.MovementType.ToString(),
                CategoryName = movement.CategoryName,
                Quantity = movement.Quantity
            };
        }
    }
}