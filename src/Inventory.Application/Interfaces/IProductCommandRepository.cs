using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Domain.Entities;

namespace Inventory.Application.Interfaces
{
    public interface IProductCommandRepository
    {
        Task<int> CreateAsync(
            Product product,
            CancellationToken cancellationToken);
    
        Task UpdateAsync(
            Product product,
            CancellationToken cancellationToken);
        
        Task DeleteAsync(
            int id,
            CancellationToken cancellationToken);
    }
}