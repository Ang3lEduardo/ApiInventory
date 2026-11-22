using Inventory.Application.DTOs;
using Inventory.Application.InventoryMovements.Commands.CreateInventoryMovement;
using Inventory.Application.InventoryMovements.Queries.GetInventoryMovementByProductId;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/inventory-movements")]
public class InventoryMovementController : ControllerBase
{
    private readonly CreateInventoryMovementCommandHandler _createHandler;
    private readonly GetInventoryMovementByProductIdQueryHandler _getByProductIdHandler;

    public InventoryMovementController(
        CreateInventoryMovementCommandHandler createHandler,
        GetInventoryMovementByProductIdQueryHandler getByProductIdHandler)
    {
        _createHandler = createHandler;
        _getByProductIdHandler = getByProductIdHandler;
    }

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

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateInventoryMovementCommand command,
        CancellationToken cancellationToken)
    {
        await _createHandler.Handle(command, cancellationToken);

        return NoContent();
    }
}
