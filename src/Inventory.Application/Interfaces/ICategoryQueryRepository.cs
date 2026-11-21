using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Domain.Entities;

namespace Inventory.Application.Interfaces
{
    public interface ICategoryQueryRepository
    {
        Task<List<Category>> GetAsync(
            CancellationToken cancellationToken);
    }
}