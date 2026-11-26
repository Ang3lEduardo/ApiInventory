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
                    UPDATE Products
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
                    throw new InvalidOperationException("Stock insuficiente o producto no encontrado.");

                await connection.ExecuteAsync(
                    """
                    INSERT INTO InventoryMovements
                        (ProductId, Quantity, MovementType, MovementDate)
                    VALUES
                        (@ProductId, @Quantity, @MovementType, SYSUTCDATETIME())
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