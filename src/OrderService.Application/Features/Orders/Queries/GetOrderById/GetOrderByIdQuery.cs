using System;
using System.Collections.Generic;
using System.Text;


using MediatR;

namespace OrderService.Application.Features.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(Guid OrderId)
    : IRequest<OrderDto?>;

