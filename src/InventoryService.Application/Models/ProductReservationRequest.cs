using System;
using System.Collections.Generic;
using System.Text;


namespace InventoryService.Application.Models;

public record ProductReservationRequest(
    Guid ProductId,
    int Quantity
);

