using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Queries.EF;

public class CategoryQueryRepository : ICategoryQueryRepository
{
    private readonly InventoryDbContext _dbContext;

    public CategoryQueryRepository(InventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<Category>> GetAllAsync(CancellationToken cancellationToken)
    {
        return _dbContext.Categories
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task<Category?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return _dbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(category => category.Id == id, cancellationToken);
    }
}
