using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Application.DTOs;
using Inventory.Domain.Entities;

namespace Inventory.Application.Interfaces
{
    public interface IInventoryMovementQueryRepository
    {
        Task<List<InventoryMovementDto>> GetByProductIdAsync(
            int productId,
            CancellationToken cancellationToken);
    }
}