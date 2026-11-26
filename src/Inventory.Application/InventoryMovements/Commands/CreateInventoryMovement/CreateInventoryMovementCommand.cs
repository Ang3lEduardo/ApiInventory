

using System.ComponentModel.DataAnnotations;
using Inventory.Domain.Enums;

namespace Inventory.Application.InventoryMovements.Commands.CreateInventoryMovement
{
    /// <summary>
    /// Command to register an inventory movement.
    /// </summary>
    /// <param name="ProductId">Identifier of the product.</param>
    /// <param name="Quantity">Quantity of units to move.</param>
    /// <param name="MovementType">1 = Increase, 2 = Decrease.</param>
    public sealed record CreateInventoryMovementCommand(
        int ProductId, 
        int Quantity, 
        [EnumDataType(typeof(MovementType))]
        MovementType MovementType);
}