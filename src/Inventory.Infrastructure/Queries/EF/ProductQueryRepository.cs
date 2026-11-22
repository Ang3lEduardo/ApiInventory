using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Queries.EF;

public class ProductQueryRepository : IProductQueryRepository
{
    private readonly InventoryDbContext _dbContext;

    public ProductQueryRepository(InventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<Product>> GetAllAsync(CancellationToken cancellationToken)
    {
        return _dbContext.Products
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task<Product?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return _dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(product => product.Id == id, cancellationToken);
    }
}
