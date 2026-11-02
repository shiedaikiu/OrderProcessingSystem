using System;
using System.Collections.Generic;
using System.Text;


namespace OrderService.Application.Features.Orders.Queries.GetOrderById;

public record OrderItemDto(
    Guid ProductId,
    int Quantity);

public record OrderDto(
    Guid Id,
    Guid CustomerId,
    string Status,
    DateTime CreatedAt,
    List<OrderItemDto> Items);

