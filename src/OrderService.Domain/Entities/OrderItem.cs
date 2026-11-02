using System;
using System.Collections.Generic;
using System.Text;


namespace OrderService.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; private set; }

    public Guid ProductId { get; private set; }

    public int Quantity { get; private set; }

    private OrderItem()
    {
    }

    public OrderItem(Guid productId, int quantity)
    {
        if (productId == Guid.Empty)
            throw new ArgumentException("ProductId is required.");

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        Id = Guid.NewGuid();
        ProductId = productId;
        Quantity = quantity;
    }
}
