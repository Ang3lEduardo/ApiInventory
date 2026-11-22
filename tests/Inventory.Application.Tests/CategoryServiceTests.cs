using Inventory.Application.Categories.Commands.CreateCategory;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;
using Xunit;

namespace Inventory.Application.Tests
{
    public class CategoryServiceTests
    {
        [Fact]
        public async Task HandleAsync_CreatesCategoryAndReturnsRepositoryId()
        {
            var repository = new FakeCategoryCommandRepository { CreatedCategoryId = 42 };
            var handler = new CreateCategoryCommandHandler(repository);
            using var cancellationSource = new CancellationTokenSource();

            var result = await handler.HandleAsync(
                new CreateCategoryCommand("Hardware"),
                cancellationSource.Token);

            Assert.Equal(42, result);
            Assert.Equal("Hardware", repository.CreatedCategory?.Name);
            Assert.Equal(cancellationSource.Token, repository.CancellationToken);
        }

        private sealed class FakeCategoryCommandRepository : ICategoryCommandRepository
        {
            public int CreatedCategoryId { get; init; }
            public Category? CreatedCategory { get; private set; }
            public CancellationToken CancellationToken { get; private set; }

            public Task<int> CreateAsync(Category category, CancellationToken cancellationToken)
            {
                CreatedCategory = category;
                CancellationToken = cancellationToken;
                return Task.FromResult(CreatedCategoryId);
            }

            public Task UpdateAsync(Category category, CancellationToken cancellationToken) =>
                Task.CompletedTask;

            public Task DeleteAsync(int id, CancellationToken cancellationToken) =>
                Task.CompletedTask;
        }
    }
}