using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Domain.Entities;
using Inventory.Domain.Enums;

namespace Inventory.Application.Interfaces
{
    public interface IInventoryMovementCommandRepository
    {
        Task<int> CreateAsync(
        Product product,
        int quantity,
        MovementType movementType,
        CancellationToken cancellationToken);
    }
}