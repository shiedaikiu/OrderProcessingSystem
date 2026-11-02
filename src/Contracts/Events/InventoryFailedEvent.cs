using System;
using System.Collections.Generic;
using System.Text;


namespace Contracts.Events;

public record InventoryFailedEvent
{
    public Guid OrderId { get; init; }

    public string Reason { get; init; } = string.Empty;

    public DateTime FailedAt { get; init; }
}

