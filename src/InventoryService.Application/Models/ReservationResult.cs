using System;
using System.Collections.Generic;
using System.Text;


namespace InventoryService.Application.Models;

public record ReservationResult(
    bool Success,
    string? Reason
);

