using GrainInterfaces;
using GrainInterfaces.Events;
using GrainInterfaces.Model;
using Microsoft.Extensions.Logging;
using Orleans;

namespace Grains;

public class CoffeShop : Grain, ICoffeShop
{
    private readonly ILogger<CoffeShop> _logger;
    private readonly List<IOrder> _orders = new();
    private readonly List<IBarista> _baristas = new();

    private readonly List<ICoffeMachine> _coffeMachines = new();

    public CoffeShop(ILogger<CoffeShop> logger)
    {
        _logger = logger;
        
    }


    /// <inheritdoc />
    public async Task OpenShop()
    {
        for (var i = 0; i < 3; i++)
        {
            var cm = GrainFactory.GetGrain<ICoffeMachine>(Guid.NewGuid());
            _coffeMachines.Add(cm);
        }
        
        //for (var i = 0; i < 3; i++)
        {
            var barista = GrainFactory.GetGrain<IBarista>(Guid.NewGuid());
            await barista.StartWorking(this);
            _baristas.Add(barista);
        }
    }

    /// <inheritdoc />
    public Task<Menu> GetMenu()
    {
        return Task.FromResult(new Menu());
    }

    /// <inheritdoc />
    public Task<Product> GetProducts(IOrder order)
    {
        _logger.LogInformation("Order is delivered to customer");
        
        _orders.Remove(order);

        return null;
    }

    /// <inheritdoc />
    public async Task AddOrder(IOrder order)
    {
        _logger.LogInformation("New order received");
        _orders.Add(order);
        
        var stream = GetStreamProvider("SMSProvider").GetStream<ProductRequestEvent>(this.GetPrimaryKey(), "AddOrder");            

        foreach (var orderItem in await order.Items())
        {
            await stream.OnNextAsync(new ProductRequestEvent()
            {
                Item = orderItem,
                OrderId = order.GetPrimaryKey()
            });
        }
    }

    /// <inheritdoc />
    public Task<ICoffeMachine> UseCoffeMachine()
    {
        _logger.LogInformation("Someone is using a coffemachine");

        var coffeMachine = _coffeMachines[0];
        _coffeMachines.Remove(coffeMachine);
        return Task.FromResult(coffeMachine);
    }

    /// <inheritdoc />
    public Task LeaveCoffeMachine(ICoffeMachine coffeMachine)
    {
        _logger.LogInformation("Someone stopped using a coffemachine");

        _coffeMachines.Add(coffeMachine);

        return Task.CompletedTask;
    }
}