using GrainInterfaces.Model;

namespace GrainInterfaces.Events;

public class ProductRequestEvent
{
    public Guid OrderId { get; set; } // event so reference is not kept
    public OrderItem Item { get; set; }
}