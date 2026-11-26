using Dapper;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Persistence;

namespace Inventory.Infrastructure.Commands.Dapper
{
    public class ProductCommandRepository : IProductCommandRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public ProductCommandRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> CreateAsync(
            Product product,
            CancellationToken cancellationToken = default)
        {
            const string sql = """
                INSERT INTO Products(Name,Stock,CategoryId)
                VALUES(@Name,@Stock,@CategoryId);

                SELECT CAST(SCOPE_IDENTITY() AS int);
                """;

            using var connection = _connectionFactory.CreateConnection();

            return await connection.ExecuteScalarAsync<int>(
                new CommandDefinition(
                    sql,
                    product,
                    cancellationToken: cancellationToken));
        }

        public async Task UpdateAsync(
            Product product,
            CancellationToken cancellationToken = default)
        {
            const string sql = """
                UPDATE Products
                SET Name = @Name,
                    Stock = @Stock,
                    CategoryId = @CategoryId
                WHERE Id = @Id;
                """;

            using var connection = _connectionFactory.CreateConnection();

            await connection.ExecuteAsync(
                new CommandDefinition(
                    sql,
                    product,
                    cancellationToken: cancellationToken));
        }

        public async Task DeleteAsync(
            int productId,
            CancellationToken cancellationToken = default)
        {
            const string sql = """
                DELETE FROM Products
                WHERE Id = @Id;
                """;

            using var connection = _connectionFactory.CreateConnection();

            await connection.ExecuteAsync(
                new CommandDefinition(
                    sql,
                    new { Id = productId },
                    cancellationToken: cancellationToken));
        }
    }
}
