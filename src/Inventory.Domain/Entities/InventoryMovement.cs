using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Domain.Enums;

namespace Inventory.Domain.Entities
{
    public class InventoryMovement
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public required MovementType MovementType { get; set; }
        public DateTime MovementDate { get; set; }

        public static InventoryMovement Create(
            int id,
            int productId,
            int quantity,
            MovementType movementType)
        {
            if(quantity <= 0)
            {
                throw new ArgumentException(
                    "Erro en el valor de la cantidad.",
                    nameof(quantity));
            }

            return new InventoryMovement
            {
                Id = id,
                ProductId = productId,
                Quantity = quantity,
                MovementType = movementType
            };
        }
    }
}