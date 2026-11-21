using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Application.Interfaces;
using Inventory.Domain.Enums;

namespace Inventory.Application.InventoryMovements.Commands.CreateInventoryMovement
{
    public class CreateInventoryMovementCommandHandler
    {
        private readonly IInventoryMovementCommandRepository _inventoryMovementCommandRepository;

        public CreateInventoryMovementCommandHandler(IInventoryMovementCommandRepository inventoryMovementCommandRepository)
        {
            _inventoryMovementCommandRepository = inventoryMovementCommandRepository;
        }

        public async Task Handle(CreateInventoryMovementCommand command, CancellationToken cancellationToken)
        {
            var inventoryMovement = new Domain.Entities.InventoryMovement
            {
                ProductId = command.ProductId,
                Quantity = command.Quantity,
                MovementType = (MovementType)command.MovementType
            };

            await _inventoryMovementCommandRepository.CreateAsync(inventoryMovement, cancellationToken);
        }
    }
}