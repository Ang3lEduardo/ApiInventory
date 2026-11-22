using Inventory.Application.Categories.Commands.CreateCategory;
using Inventory.Application.Categories.Commands.DeleteCategory;
using Inventory.Application.Categories.Commands.UpdateCategory;
using Inventory.Application.Categories.Queries.GetCategories;
using Inventory.Application.Categories.Queries.GetCategoryById;
using Inventory.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly CreateCategoryCommandHandler _createCategoryHandler;
    private readonly UpdateCategoryCommandHandler _updateCategoryHandler;
    private readonly DeleteCategoryCommandHandler _deleteCategoryHandler;
    private readonly GetCategoriesQueryHandler _getCategoriesHandler;
    private readonly GetCategoryByIdQueryHandler _getCategoryByIdHandler;

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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetAll(
        CancellationToken cancellationToken)
    {
        var categories = await _getCategoriesHandler.HandleAsync(
            new GetCategoriesQuery(),
            cancellationToken);

        return Ok(categories);
    }

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

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var commandWithId = command with { Id = id };

        if (commandWithId.Id != id)
        {
            return BadRequest("The route ID must match the command ID.");
        }

        await _updateCategoryHandler.HandleAsync(
            commandWithId,
            cancellationToken);

        return NoContent();
    }

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
