using GrainInterfaces.Model;
using Orleans;

namespace GrainInterfaces;

public interface IOrder : IGrainWithGuidKey
{
    Task<OrderItem[]> Items();

    Task Fulfill(Product product);
    Task AddItem(string name);

    Task SetCustomer(ICustomer customer);
}