using Contracts.Events;
using InventoryService.Application.Abstractions.Persistence;
using InventoryService.Application.Models;
using MassTransit;

namespace InventoryService.Application.Consumers;

public class OrderCreatedEventConsumer
    : IConsumer<OrderCreatedEvent>
{
    private readonly IProductRepository _productRepository;

    public OrderCreatedEventConsumer(
        IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Consume(
        ConsumeContext<OrderCreatedEvent> context)
    {
        var message = context.Message;

        var items = message.Items
            .Select(x => new ProductReservationRequest(
                x.ProductId,
                x.Quantity))
            .ToList();

        var result = await _productRepository.TryReserveItemsAsync(
            items,
            context.CancellationToken);

        if (result.Success)
        {
            await context.Publish(
                new InventoryReservedEvent
                {
                    OrderId = message.OrderId,
                    ReservedAt = DateTime.UtcNow
                },
                context.CancellationToken);
        }
        else
        {
            await context.Publish(
                new InventoryFailedEvent
                {
                    OrderId = message.OrderId,
                    Reason = result.Reason
                             ?? "Inventory reservation failed.",
                    FailedAt = DateTime.UtcNow
                },
                context.CancellationToken);
        }
    }
}