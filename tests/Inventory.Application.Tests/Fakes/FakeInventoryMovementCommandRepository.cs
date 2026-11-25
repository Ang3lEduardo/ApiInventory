using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;
using Inventory.Domain.Enums;

namespace Inventory.Application.Tests.Fakes
{
    public class FakeInventoryMovementCommandRepository : IInventoryMovementCommandRepository
    {
        private readonly List<InventoryMovement> _inventoryMovements = [];
        private readonly Dictionary<int, int> _stockByProduct = [];

        public IReadOnlyList<InventoryMovement> InventoryMovements => _inventoryMovements;

        public InventoryMovement? CreatedInventoryMovement { get; private set; }

        public CancellationToken CancellationToken { get; private set; }

        public void SetStock(int productId, int stock)
        {
            _stockByProduct[productId] = stock;
        }

        public int GetStock(int productId) =>
            _stockByProduct.TryGetValue(productId, out var stock) ? stock : 0;

        public Task CreateAsync(
            InventoryMovement inventoryMovement,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(inventoryMovement);

            if (inventoryMovement.Quantity <= 0)
            {
                throw new ArgumentException(
                    "The movement quantity must be greater than zero.",
                    nameof(inventoryMovement.Quantity));
            }

            if (inventoryMovement.MovementType == MovementType.Decrease &&
                (! _stockByProduct.TryGetValue(inventoryMovement.ProductId, out var stock) ||
                 inventoryMovement.Quantity > stock))
            {
                throw new ArgumentException(
                    "The movement quantity exceeds the available stock.",
                    nameof(inventoryMovement.Quantity));
            }

            CreatedInventoryMovement = inventoryMovement;
            CancellationToken = cancellationToken;
            _inventoryMovements.Add(inventoryMovement);

            if (inventoryMovement.MovementType == MovementType.Decrease)
            {
                _stockByProduct[inventoryMovement.ProductId] -= inventoryMovement.Quantity;
            }
            else
            {
                _stockByProduct[inventoryMovement.ProductId] =
                    GetStock(inventoryMovement.ProductId) + inventoryMovement.Quantity;
            }

            return Task.CompletedTask;
        }
    }
}