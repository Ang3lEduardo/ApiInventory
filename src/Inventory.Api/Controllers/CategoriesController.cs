using Inventory.Application.Categories.Commands.CreateCategory;
using Inventory.Application.Categories.Commands.DeleteCategory;
using Inventory.Application.Categories.Commands.UpdateCategory;
using Inventory.Application.Categories.Queries.GetCategories;
using Inventory.Application.Categories.Queries.GetCategoryById;
using Inventory.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

/// <summary>Provides endpoints for managing product categories.</summary>
[Authorize]
[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly CreateCategoryCommandHandler _createCategoryHandler;
    private readonly UpdateCategoryCommandHandler _updateCategoryHandler;
    private readonly DeleteCategoryCommandHandler _deleteCategoryHandler;
    private readonly GetCategoriesQueryHandler _getCategoriesHandler;
    private readonly GetCategoryByIdQueryHandler _getCategoryByIdHandler;

    /// <summary>Initializes a new instance of the <see cref="CategoriesController"/> class.</summary>
    public CategoriesController(
        CreateCategoryCommandHandler createCategoryHandler,
        UpdateCategoryCommandHandler updateCategoryHandler,
        DeleteCategoryCommandHandler deleteCategoryHandler,
        GetCategoriesQueryHandler getCategoriesHandler,
        GetCategoryByIdQueryHandler getCategoryByIdHandler)
    {
        _createCategoryHandler = createCategoryHandler;
        _updateCategoryHandler = updateCategoryHandler;
        _deleteCategoryHandler = deleteCategoryHandler;
        _getCategoriesHandler = getCategoriesHandler;
        _getCategoryByIdHandler = getCategoryByIdHandler;
    }

    /// <summary>Gets all categories.</summary>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The list of categories.</returns>
    /// <response code="200">Returns the categories.</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetAll(
        CancellationToken cancellationToken)
    {
        var categories = await _getCategoriesHandler.HandleAsync(
            new GetCategoriesQuery(),
            cancellationToken);

        return Ok(categories);
    }

    /// <summary>Gets a category by its identifier.</summary>
    /// <param name="id">The category identifier.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The matching category.</returns>
    /// <response code="200">Returns the category.</response>
    /// <response code="404">No category exists with the specified identifier.</response>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Category>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var category = await _getCategoryByIdHandler.HandleAsync(
            new GetCategoryByIdQuery(id),
            cancellationToken);

        return category is null ? NotFound() : Ok(category);
    }

    /// <summary>Creates a category.</summary>
    /// <param name="command">The category details.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The created category.</returns>
    /// <response code="201">Returns the created category.</response>
    [HttpPost]
    public async Task<ActionResult> Create(
        CreateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _createCategoryHandler.HandleAsync(command, cancellationToken);

        var _category = new Category
        {
            Id = id,
            Name = command.Name
        };

        return CreatedAtAction(
            nameof(GetById),
            new { id },
            _category);
    }

    /// <summary>Updates a category.</summary>
    /// <param name="id">The category identifier.</param>
    /// <param name="command">The updated category details.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>No content when the category is updated.</returns>
    /// <response code="204">The category was updated.</response>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var commandWithId = command with { Id = id };

        if (commandWithId.Id != id)
        {
            return BadRequest("El Id no es correcto.");
        }

        await _updateCategoryHandler.HandleAsync(
            commandWithId,
            cancellationToken);

        return NoContent();
    }

    /// <summary>Deletes a category.</summary>
    /// <param name="id">The category identifier.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>No content when the category is deleted.</returns>
    /// <response code="204">The category was deleted.</response>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteCategoryCommand(id);

        await _deleteCategoryHandler.HandleAsync(
            command,
            cancellationToken);

        return NoContent();
    }
}
