using System;
using System.Collections.Generic;
using System.Text;



namespace InventoryService.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public int AvailableQuantity { get; private set; }

    // Optimistic Concurrency Token
    public int Version { get; private set; }

    private Product()
    {
    }

    public Product(string name, int availableQuantity)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name is required.");

        if (availableQuantity < 0)
            throw new ArgumentException("Stock cannot be negative.");

        Id = Guid.NewGuid();
        Name = name;
        AvailableQuantity = availableQuantity;
        Version = 1;
    }

    public bool TryReserve(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException(
                "Quantity must be greater than zero.");

        if (AvailableQuantity < quantity)
            return false;

        AvailableQuantity -= quantity;
        Version++;

        return true;
    }
}


