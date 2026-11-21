using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Domain.Entities;

namespace Inventory.Application.Interfaces
{
    public interface IProductQueryRepository
    {
        Task<List<Product>> GetAllAsync(
            CancellationToken cancellationToken);

        Task<Product?> GetByIdAsync(int id, 
                            CancellationToken cancellationToken);
    }
}