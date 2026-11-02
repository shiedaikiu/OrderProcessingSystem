using System;
using System.Collections.Generic;
using System.Text;


using OrderService.Domain.Enums;

namespace OrderService.Domain.Entities;

public class Order
{
    private readonly List<OrderItem> _items = new();

    public Guid Id { get; private set; }

    public Guid CustomerId { get; private set; }

    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public OrderStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private Order()
    {
    }

    public Order(Guid customerId, IEnumerable<OrderItem> items)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentException("CustomerId is required.");

        var orderItems = items?.ToList();

        if (orderItems == null || orderItems.Count == 0)
            throw new ArgumentException("Order must contain at least one item.");

        Id = Guid.NewGuid();
        CustomerId = customerId;
        Status = OrderStatus.Pending;
        CreatedAt = DateTime.UtcNow;

        _items.AddRange(orderItems);
    }

    public void Confirm()
    {
        EnsurePending();

        Status = OrderStatus.Confirmed;
    }

    public void Fail()
    {
        EnsurePending();

        Status = OrderStatus.Failed;
    }

    private void EnsurePending()
    {
        if (Status != OrderStatus.Pending)
        {
            throw new InvalidOperationException(
                "Only pending orders can change their status.");
        }
    }
}

