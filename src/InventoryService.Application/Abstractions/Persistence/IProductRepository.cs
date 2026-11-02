using InventoryService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using InventoryService.Application.Models;
namespace InventoryService.Application.Abstractions.Persistence;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(
        Guid productId,
        CancellationToken cancellationToken);

    Task AddAsync(
        Product product,
        CancellationToken cancellationToken);

    Task SaveChangesAsync(
        CancellationToken cancellationToken);

    Task<bool> TryReserveAsync(
    Guid productId,
    int quantity,
    CancellationToken cancellationToken);

    


Task<ReservationResult> TryReserveItemsAsync(
    IReadOnlyCollection<ProductReservationRequest> items,
    CancellationToken cancellationToken
);

}

