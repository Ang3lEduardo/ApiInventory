using Dapper;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
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

        public async Task CreateAsync(
            InventoryMovement inventoryMovement,
            CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();

            try
            {
                var stockChange = inventoryMovement.MovementType == MovementType.Increase
                    ? inventoryMovement.Quantity
                    : -inventoryMovement.Quantity;

                var rowsAffected = await connection.ExecuteAsync(
                    """
                    UPDATE Product
                    SET Stock = Stock + @StockChange
                    WHERE Id = @ProductId
                    AND Stock + @StockChange >= 0
                    """,
                    new
                    {
                        inventoryMovement.ProductId,
                        StockChange = stockChange
                    },
                    transaction);

                if (rowsAffected == 0)
                    throw new InvalidOperationException("Insufficient stock or product not found.");

                await connection.ExecuteAsync(
                    """
                    INSERT INTO InventoryMovement
                        (ProductId, Quantity, Type, CreatedAt)
                    VALUES
                        (@ProductId, @Quantity, @Type, SYSUTCDATETIME())
                    """,
                    inventoryMovement,
                    transaction);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}