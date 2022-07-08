using GrainInterfaces.Model;
using Orleans;

namespace GrainInterfaces;

public interface ICoffeMachine : IGrainWithGuidKey
{
    Task<Product> MakeCoffee(OrderItem orderItem);
}