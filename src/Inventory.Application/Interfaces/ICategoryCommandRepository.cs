using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Domain.Entities;

namespace Inventory.Application.Interfaces
{
    public interface ICategoryCommandRepository
    {
        Task<int> CreateAsync(Category category,
                              CancellationToken cancellationToken);
    
        Task UpdateAsync(Category category,
                         CancellationToken cancellationToken);
        
        Task DeleteAsync(int id,
                         CancellationToken cancellationToken);
    }
}