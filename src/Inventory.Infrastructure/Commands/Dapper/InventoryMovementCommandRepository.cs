using Dapper;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Persistence;

namespace Inventory.Infrastructure.Commands.Dapper
{
    public class InventoryMovementCommandRepository : IInventoryMovementCommandRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public InventoryMovementCommandRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> CreateAsync(
            InventoryMovement inventoryMovement,
            CancellationToken cancellationToken)
        {
            const string sql = """
                INSERT INTO InventoryMovement(
                    ProductId,
                    Quantity,
                    MovementType,
                    MovementDate)
                VALUES(
                    @ProductId,
                    @Quantity,
                    @MovementType,
                    GETUTCDATE());

                SELECT CAST(SCOPE_IDENTITY() AS int);
                """;

            using var connection = _connectionFactory.CreateConnection();

            return await connection.ExecuteScalarAsync<int>(
                new CommandDefinition(
                    sql,
                    inventoryMovement,
                    cancellationToken: cancellationToken));
        }
    }
}