using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Application.DTOs;
using Inventory.Application.Interfaces;
using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Queries.EF
{
    public class InventoryMovementQueryRepository : IInventoryMovementQueryRepository
    {
        private readonly InventoryDbContext _dbContext;

        public InventoryMovementQueryRepository(InventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<InventoryMovementDto>> GetByProductIdAsync(
            int productId,
            CancellationToken cancellationToken)
        {
            return (
                from movement in _dbContext.InventoryMovements.AsNoTracking()
                join product in _dbContext.Products.AsNoTracking()
                    on movement.ProductId equals product.Id
                join category in _dbContext.Categories.AsNoTracking()
                    on product.CategoryId equals category.Id
                where movement.ProductId == productId
                select new InventoryMovementDto
                {
                    ProductName = product.Name,
                    CategoryName = category.Name,
                    Quantity = movement.Quantity,
                    MovementType = movement.MovementType.ToString()
                })
                .ToListAsync(cancellationToken);
        }
    }
}