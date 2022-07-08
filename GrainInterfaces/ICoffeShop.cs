using GrainInterfaces.Model;
using Orleans;

namespace GrainInterfaces;

public interface ICoffeShop : IGrainWithGuidKey
{
    Task OpenShop();

    Task<Menu> GetMenu();
    Task<Product> GetProducts(IOrder order);
    Task AddOrder(IOrder customer);

    Task<ICoffeMachine> UseCoffeMachine();
    Task LeaveCoffeMachine(ICoffeMachine coffeMachine);
}