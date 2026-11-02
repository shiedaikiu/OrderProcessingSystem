using InventoryService.Application.Abstractions.Persistence;
using InventoryService.Application.Models;
using InventoryService.Domain.Entities;
using InventoryService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryService.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly InventoryDbContext _context;

    public ProductRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(
        Guid productId,
        CancellationToken cancellationToken)
    {
        return await _context.Products
            .FirstOrDefaultAsync(
                x => x.Id == productId,
                cancellationToken);
    }

    public async Task AddAsync(
        Product product,
        CancellationToken cancellationToken)
    {
        await _context.Products.AddAsync(
            product,
            cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> TryReserveAsync(
        Guid productId,
        int quantity,
        CancellationToken cancellationToken)
    {
        var product = await GetByIdAsync(
            productId,
            cancellationToken);

        if (product is null)
            return false;

        if (!product.TryReserve(quantity))
            return false;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
    }


    public async Task<ReservationResult> TryReserveItemsAsync(
        IReadOnlyCollection<ProductReservationRequest> items,
        CancellationToken cancellationToken)
    {
        if (items.Count == 0)
            return new ReservationResult(false, "Order has no items.");

        var groupedItems = items
            .GroupBy(x => x.ProductId)
            .Select(g => new ProductReservationRequest(
                g.Key,
                g.Sum(x => x.Quantity)))
            .ToList();

        if (groupedItems.Any(x => x.Quantity <= 0))
            return new ReservationResult(false, "Invalid quantity.");

        var productIds = groupedItems
            .Select(x => x.ProductId)
            .ToList();

        var products = await _context.Products
            .Where(x => productIds.Contains(x.Id))
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

        if (products.Count != productIds.Count)
        {
            return new ReservationResult(
                false,
                "One or more products were not found.");
        }

        foreach (var item in groupedItems)
        {
            var product = products.Single(x => x.Id == item.ProductId);

            if (product.AvailableQuantity < item.Quantity)
            {
                return new ReservationResult(
                    false,
                    $"Insufficient stock for product {item.ProductId}.");
            }
        }

        foreach (var item in groupedItems)
        {
            var product = products.Single(x => x.Id == item.ProductId);

            if (!product.TryReserve(item.Quantity))
            {
                return new ReservationResult(
                    false,
                    $"Failed to reserve product {item.ProductId}.");
            }
        }

        try
        {
            await _context.SaveChangesAsync(cancellationToken);

            return new ReservationResult(true, null);
        }
        catch (DbUpdateConcurrencyException)
        {
            return new ReservationResult(
                false,
                "Inventory was modified concurrently. Please retry.");
        }
    }


}

