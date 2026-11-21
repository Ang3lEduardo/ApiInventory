using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand
    {
        public required string Name { get; set; }
    }
}