using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;

namespace Inventory.Application.Tests.Fakes
{
    public class FakeCategoryCommandRepository : ICategoryCommandRepository
    {
        private readonly List<Category> _categories = [];

        public IReadOnlyList<Category> Categories => _categories;

        public void Add(Category category)
        {
            _categories.Add(category);
        }

        public Task<int> CreateAsync(Category category,
                                     CancellationToken cancellationToken)
        {
            _categories.Add(category);

            return Task.FromResult(category.Id);
        }

        public Task UpdateAsync(Category category,
                                CancellationToken cancellationToken)
        {
            var existingCategory = _categories
                .FirstOrDefault(x => x.Id == category.Id);

            if (existingCategory is null)
            {
                throw new InvalidOperationException(
                    $"Category with id {category.Id} was not found.");
            }

            existingCategory.Name = category.Name;

            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id,
                                CancellationToken cancellationToken)
        {
            var category = _categories
                .FirstOrDefault(x => x.Id == id);

            if (category is null)
            {
                throw new InvalidOperationException(
                    $"Category with id {id} was not found.");
            }

            _categories.Remove(category);

            return Task.CompletedTask;
        }
    }
}
