using Inventory.Application.DTOs;
using Inventory.Application.InventoryMovements.Commands.CreateInventoryMovement;
using Inventory.Application.InventoryMovements.Queries.GetInventoryMovementByProductId;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

/// <summary>Provides endpoints for recording and retrieving inventory movements.</summary>
[Authorize]
[ApiController]
[Route("api/inventory-movements")]
public class InventoryMovementController : ControllerBase
{
    private readonly CreateInventoryMovementCommandHandler _createHandler;
    private readonly GetInventoryMovementByProductIdQueryHandler _getByProductIdHandler;

    /// <summary>Initializes a new instance of the <see cref="InventoryMovementController"/> class.</summary>
    public InventoryMovementController(
        CreateInventoryMovementCommandHandler createHandler,
        GetInventoryMovementByProductIdQueryHandler getByProductIdHandler)
    {
        _createHandler = createHandler;
        _getByProductIdHandler = getByProductIdHandler;
    }

    /// <summary>Gets inventory movements for a product.</summary>
    /// <param name="productId">The product identifier.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The inventory movements for the specified product.</returns>
    /// <response code="200">Returns the product's inventory movements.</response>
    [HttpGet("product/{productId:int}")]
    public async Task<ActionResult<IEnumerable<InventoryMovementDto>>> GetByProductId(
        int productId,
        CancellationToken cancellationToken)
    {
        var movements = await _getByProductIdHandler.Handle(
            new GetInventoryMovementByProductIdQuery(productId),
            cancellationToken);

        return Ok(movements);
    }

    /// <summary>Records an inventory movement.</summary>
    /// <param name="command">The inventory movement details.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>No content when the movement is recorded.</returns>
    /// <response code="204">The inventory movement was recorded.</response>
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateInventoryMovementCommand command,
        CancellationToken cancellationToken)
    {
        await _createHandler.Handle(command, cancellationToken);

        return NoContent();
    }
}
