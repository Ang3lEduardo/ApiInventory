

namespace Inventory.Application.InventoryMovements.Commands.CreateInventoryMovement
{
    public sealed record CreateInventoryMovementCommand(int ProductId, int Quantity, int MovementType);
}