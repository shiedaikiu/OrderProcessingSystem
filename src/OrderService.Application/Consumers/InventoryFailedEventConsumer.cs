using System;
using System.Collections.Generic;
using System.Text;


using Contracts.Events;
using MassTransit;
using OrderService.Application.Abstractions.Persistence;

namespace OrderService.Application.Consumers;

public class InventoryFailedEventConsumer
    : IConsumer<InventoryFailedEvent>
{
    private readonly IOrderRepository _orderRepository;

    public InventoryFailedEventConsumer(
        IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Consume(
        ConsumeContext<InventoryFailedEvent> context)
    {
        var order = await _orderRepository.GetByIdAsync(
            context.Message.OrderId,
            context.CancellationToken);

        if (order is null)
            return;

        if (order.Status != Domain.Enums.OrderStatus.Pending)
            return;

        order.Fail();

        await _orderRepository.UpdateAsync(
            order,
            context.CancellationToken);
    }
}

