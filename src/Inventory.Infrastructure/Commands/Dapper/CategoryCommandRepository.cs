using Dapper;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Persistence;

namespace Inventory.Infrastructure.Commands.Dapper
{
    public class CategoryCommandRepository : ICategoryCommandRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public CategoryCommandRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> CreateAsync(
            Category category,
            CancellationToken cancellationToken)
        {
            const string sql = """
                INSERT INTO Categories(Name)
                VALUES(@Name);

                SELECT CAST(SCOPE_IDENTITY() AS int);
                """;

            using var connection = _connectionFactory.CreateConnection();

            return await connection.ExecuteScalarAsync<int>(
                new CommandDefinition(
                    sql,
                    category,
                    cancellationToken: cancellationToken));
        }

        public async Task UpdateAsync(
            Category category,
            CancellationToken cancellationToken)
        {
            const string sql = """
                UPDATE Categories
                SET Name = @Name
                WHERE Id = @Id;
                """;

            using var connection = _connectionFactory.CreateConnection();

            await connection.ExecuteAsync(
                new CommandDefinition(
                    sql,
                    category,
                    cancellationToken: cancellationToken));
        }

        public async Task DeleteAsync(
            int id,
            CancellationToken cancellationToken)
        {
            const string sql = """
                DELETE FROM Categories
                WHERE Id = @Id;
                """;

            using var connection = _connectionFactory.CreateConnection();

            await connection.ExecuteAsync(
                new CommandDefinition(
                    sql,
                    new { Id = id },
                    cancellationToken: cancellationToken));
        }
    }
}