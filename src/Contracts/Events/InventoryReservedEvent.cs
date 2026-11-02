using System;
using System.Collections.Generic;
using System.Text;


namespace Contracts.Events;

public record InventoryReservedEvent
{
    public Guid OrderId { get; init; }

    public DateTime ReservedAt { get; init; }
}
