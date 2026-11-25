using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int CategoryId { get; set; }
        public int Stock { get; set; }
        public Category Category { get; set; } = null!;

        public static Product Create(
            string name,
            int categoryId,
            int stock)
        {
            if (stock < 0)
            {
                throw new ArgumentException(
                    "El stock no puede ser negativo.",
                    nameof(stock));
            }

            return new Product
            {
                Name = name,
                CategoryId = categoryId,
                Stock = stock
            };
        }
    }
}