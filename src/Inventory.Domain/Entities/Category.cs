using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public ICollection<Product> Products { get; set; } = [];

        public static Category Create (
            string name
        )
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(
                    "El nombre es requerido.",
                    nameof(name));
            }

            return new Category
            {
                Name = name
            };
        }
    }
}