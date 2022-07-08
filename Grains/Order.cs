using GrainInterfaces;
using GrainInterfaces.Model;
using Microsoft.Extensions.Logging;
using Orleans;

namespace Grains;

public class Order : Grain, IOrder
{
    private ILogger<Order> _logger;

    private readonly List<OrderItem> _items = new();
    private ICustomer _customer;

    public Order(ILogger<Order> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public Task<OrderItem[]> Items()
    {
        return Task.FromResult(_items.ToArray());
    }

    /// <inheritdoc />
    public async Task Fulfill(Product product)
    {
        _items.First(x => x.Fulfilled == false && x.Name == product.Name).Fulfilled = true;
        if (_items.All(x => x.Fulfilled))
        {
            await _customer.NotifyOrderComplete(this);
        }
    }

    /// <inheritdoc />
    public Task AddItem(string name)
    {
        _items.Add(new OrderItem()
        {
            Name = name,
        });
        
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task SetCustomer(ICustomer customer)
    {
        _logger.LogInformation("Set customer on order");
        _customer = customer;
        return Task.CompletedTask;
    }
}