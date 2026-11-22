using Inventory.Application.Products.Commands.CreateProduct;
using Inventory.Application.Products.Commands.DeleteProduct;
using Inventory.Application.Products.Commands.UpdateProduct;
using Inventory.Application.Products.Queries.GetProductById;
using Inventory.Application.Products.Queries.GetProducts;
using Inventory.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly CreateProductCommandHandler _createProductHandler;
    private readonly UpdateProductCommandHandler _updateProductHandler;
    private readonly DeleteProductCommandHandler _deleteProductHandler;
    private readonly GetProductsQueryHandler _getProductsHandler;
    private readonly GetProductByIdQueryHandler _getProductByIdHandler;

    public ProductsController(
        CreateProductCommandHandler createProductHandler,
        UpdateProductCommandHandler updateProductHandler,
        DeleteProductCommandHandler deleteProductHandler,
        GetProductsQueryHandler getProductsHandler,
        GetProductByIdQueryHandler getProductByIdHandler)
    {
        _createProductHandler = createProductHandler;
        _updateProductHandler = updateProductHandler;
        _deleteProductHandler = deleteProductHandler;
        _getProductsHandler = getProductsHandler;
        _getProductByIdHandler = getProductByIdHandler;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll(
        CancellationToken cancellationToken)
    {
        var products = await _getProductsHandler.Handle(
            new GetProductsQuery(),
            cancellationToken);

        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var product = await _getProductByIdHandler.HandleAsync(
            new GetProductByIdQuery(id),
            cancellationToken);

        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create(
        [FromBody] CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _createProductHandler.HandleAsync(command, cancellationToken);
        var product = new Product
        {
            Id = id,
            Name = command.Name,
            CategoryId = command.CategoryId,
            Stock = command.Stock
        };

        return CreatedAtAction(nameof(GetById), new { id }, product);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateProductCommand command,
        CancellationToken cancellationToken)
    {
        var commandWithId = command with { Id = id };

        await _updateProductHandler.HandleAsync(commandWithId, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteProductCommand(id);
        await _deleteProductHandler.HandleAsync(
            command,
            cancellationToken);

        return NoContent();
    }
}
