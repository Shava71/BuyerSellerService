using MediatR;
using Moq;
using ShopService.Application.Services.Implementations;
using ShopService.Application.Services.Interfaces;
using ShopService.Domain.Entities;
using ShopService.Domain.Events;
using ShopService.Domain.Repositories;
using ShopService.Domain.ValueObject;

namespace ShopService.Tests.Unit;

public class PurchaseServiceTests
{
    [Fact]
    public async Task Purchase_Publishes_Event_And_UsesRepo()
    {
        // Arrange
        var repoMock = new Mock<IItemsRepository>();
        var mediatorMock = new Mock<IMediator>();
        var metricsMock = new Mock<IMetricService>();

        // метрика должна возвращать IDisposable
        var timerMock = new Mock<IDisposable>();
        metricsMock.Setup(m => m.BeginPurchaseTimer())
            .Returns(timerMock.Object);

        // входящие id
        var ids = new[] { Guid.NewGuid(), Guid.NewGuid() };

        // создаём Purchase, который вернёт репозиторий
        var purchase = new Purchase
        (
            id: Guid.NewGuid(),
            purchasedAt: DateTime.UtcNow,
            items: new List<Item>
            {
                new Item ( id: ids[0], name: "jew", price: 10m, category: Category.Jewelry ),
                new Item ( id: ids[1], name: "cloth", price: 20m, category: Category.Clothes)
            }
        );

        repoMock
            .Setup(r => r.PurchaseItemWithLockAsync(ids, It.IsAny<CancellationToken>()))
            .ReturnsAsync(purchase);

        var svc = new PurchaseService(repoMock.Object, mediatorMock.Object, metricsMock.Object);

        // Act
        await svc.PurchaseAsync(ids);

        // Assert:
        // Репо вызван один раз
        repoMock.Verify(
            r => r.PurchaseItemWithLockAsync(ids, It.IsAny<CancellationToken>()),
            Times.Once);

        // Метрика началась
        metricsMock.Verify(
            m => m.BeginPurchaseTimer(),
            Times.Once);

        // Таймер освобождён
        timerMock.Verify(t => t.Dispose(), Times.Once);

        // Опубликованы 2 события
        mediatorMock.Verify(
            m => m.Publish(It.IsAny<ItemPurchasedEvent>(), It.IsAny<CancellationToken>()),
            Times.Exactly(2));
    }
}