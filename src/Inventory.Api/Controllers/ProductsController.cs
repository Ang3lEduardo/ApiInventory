using Inventory.Application.Products.Commands.CreateProduct;
using Inventory.Application.Products.Commands.DeleteProduct;
using Inventory.Application.Products.Commands.UpdateProduct;
using Inventory.Application.Products.Queries.GetProductById;
using Inventory.Application.Products.Queries.GetProducts;
using Inventory.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

/// <summary>Provides endpoints for managing products.</summary>
[Authorize]
[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly CreateProductCommandHandler _createProductHandler;
    private readonly UpdateProductCommandHandler _updateProductHandler;
    private readonly DeleteProductCommandHandler _deleteProductHandler;
    private readonly GetProductsQueryHandler _getProductsHandler;
    private readonly GetProductByIdQueryHandler _getProductByIdHandler;

    /// <summary>Initializes a new instance of the <see cref="ProductsController"/> class.</summary>
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

    /// <summary>Gets all products.</summary>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The list of products.</returns>
    /// <response code="200">Returns the products.</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll(
        CancellationToken cancellationToken)
    {
        var products = await _getProductsHandler.Handle(
            new GetProductsQuery(),
            cancellationToken);

        return Ok(products);
    }

    /// <summary>Gets a product by its identifier.</summary>
    /// <param name="id">The product identifier.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The matching product.</returns>
    /// <response code="200">Returns the product.</response>
    /// <response code="404">No product exists with the specified identifier.</response>
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

    /// <summary>Creates a product.</summary>
    /// <param name="command">The product details.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The created product.</returns>
    /// <response code="201">Returns the created product.</response>
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

    /// <summary>Updates a product.</summary>
    /// <param name="id">The product identifier.</param>
    /// <param name="command">The updated product details.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>No content when the product is updated.</returns>
    /// <response code="204">The product was updated.</response>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateProductCommand command,
        CancellationToken cancellationToken)
    {
        var commandWithId = command with { Id = id };

        if (commandWithId.Id != id)
        {
            return BadRequest("El Id no es correcto.");
        }

        await _updateProductHandler.HandleAsync(commandWithId, cancellationToken);
        return NoContent();
    }

    /// <summary>Deletes a product.</summary>
    /// <param name="id">The product identifier.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>No content when the product is deleted.</returns>
    /// <response code="204">The product was deleted.</response>
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
