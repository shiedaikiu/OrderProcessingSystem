using MediatR;
using OrderService.Application.Abstractions.Persistence;
using OrderService.Domain.Entities;
using Contracts.Events;
using MassTransit;

namespace OrderService.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler
    : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IPublishEndpoint publishEndpoint)
    {
        _orderRepository = orderRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Guid> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var orderItems = request.Items
            .Select(item => new OrderItem(
                item.ProductId,
                item.Quantity))
            .ToList();

        var order = new Order(
            request.CustomerId,
            orderItems);

        await _orderRepository.AddAsync(
            order,
            cancellationToken);

        var orderCreatedEvent = new OrderCreatedEvent
        {
            OrderId = order.Id,
            CustomerId = order.CustomerId,
            CreatedAt = order.CreatedAt,
            Items = order.Items.Select(item =>
                new OrderItemMessage
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList()
        };

        await _publishEndpoint.Publish(
            orderCreatedEvent,
            cancellationToken);

        await _orderRepository.SaveChangesAsync(
            cancellationToken);

        return order.Id;
    }
}


