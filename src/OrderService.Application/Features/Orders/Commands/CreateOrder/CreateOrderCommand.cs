using System;
using System.Collections.Generic;
using System.Text;


using MediatR;

namespace OrderService.Application.Features.Orders.Commands.CreateOrder;

public record CreateOrderItemRequest(
    Guid ProductId,
    int Quantity);

public record CreateOrderCommand(
    Guid CustomerId,
    List<CreateOrderItemRequest> Items
) : IRequest<Guid>;

