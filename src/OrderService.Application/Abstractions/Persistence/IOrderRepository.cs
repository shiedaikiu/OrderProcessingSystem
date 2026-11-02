using System;
using System.Collections.Generic;
using System.Text;


using OrderService.Domain.Entities;

namespace OrderService.Application.Abstractions.Persistence;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken);

    Task<Order?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);

    Task UpdateAsync(
        Order order,
        CancellationToken cancellationToken);

}

