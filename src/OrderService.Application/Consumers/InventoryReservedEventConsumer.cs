using System;
using System.Collections.Generic;
using System.Text;


using Contracts.Events;
using MassTransit;
using OrderService.Application.Abstractions.Persistence;

namespace OrderService.Application.Consumers;

public class InventoryReservedEventConsumer
    : IConsumer<InventoryReservedEvent>
{
    private readonly IOrderRepository _orderRepository;

    public InventoryReservedEventConsumer(
        IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Consume(
        ConsumeContext<InventoryReservedEvent> context)
    {
        var order = await _orderRepository.GetByIdAsync(
            context.Message.OrderId,
            context.CancellationToken);

        if (order is null)
            return;

        if (order.Status != Domain.Enums.OrderStatus.Pending)
            return;

        order.Confirm();

        await _orderRepository.UpdateAsync(
            order,
            context.CancellationToken);
    }
}

