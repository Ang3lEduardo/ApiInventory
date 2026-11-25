using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Application.InventoryMovements.Commands.CreateInventoryMovement;
using Inventory.Application.Tests.Fakes;
using Inventory.Domain.Enums;
using Xunit;

namespace Inventory.Application.Tests
{
    public class InventoryServiceTests
    {
        [Fact]
        public async Task CreateInventoryMovement_WithValidData_IsCreatedSuccessfully()
        {
            var repository = new FakeInventoryMovementCommandRepository();
            repository.SetStock(7, 10);
            var handler = new CreateInventoryMovementCommandHandler(repository);
            using var cancellationSource = new CancellationTokenSource();

            await handler.Handle(
                new CreateInventoryMovementCommand(7, 5, (int)MovementType.Decrease),
                cancellationSource.Token);

            Assert.Single(repository.InventoryMovements);
            Assert.Equal(7, repository.CreatedInventoryMovement?.ProductId);
            Assert.Equal(5, repository.CreatedInventoryMovement?.Quantity);
            Assert.Equal(MovementType.Decrease, repository.CreatedInventoryMovement?.MovementType);
            Assert.Equal(cancellationSource.Token, repository.CancellationToken);
            Assert.Equal(5, repository.GetStock(7));
        }

        [Fact]
        public async Task CreateInventoryMovement_WithExceedingQuantity_ThrowsArgumentException()
        {
            var repository = new FakeInventoryMovementCommandRepository();
            repository.SetStock(7, 10);
            var handler = new CreateInventoryMovementCommandHandler(repository);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(
                    new CreateInventoryMovementCommand(7, 11, (int)MovementType.Decrease),
                    CancellationToken.None));

            Assert.Empty(repository.InventoryMovements);
            Assert.Equal(10, repository.GetStock(7));
        }
    }
}