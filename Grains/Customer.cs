using GrainInterfaces;
using Microsoft.Extensions.Logging;
using Orleans;

namespace Grains;

public class Customer : Grain, ICustomer
{
    private readonly ILogger<Customer> _logger;
    private string _phoneNumber;
    private readonly List<IOrder> _orders = new();

    public Customer(ILogger<Customer> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public Task<IOrder[]> Orders()
    {
        _logger.LogInformation("Get Orders on Customer");
        return Task.FromResult(_orders.ToArray());
    }

    /// <inheritdoc />
    public async Task NotifyOrderComplete(IOrder order)
    {
        _logger.LogInformation("Notify Order complete on Customer");

        var notifier = GrainFactory.GetGrain<INotifier>(0);
        await notifier.Notify(_phoneNumber, "Your order is complete");
    }

    /// <inheritdoc />
    public Task AddPhoneNumber(string phoneNumber)
    {
        _logger.LogInformation("Set phone number on Customer");

        _phoneNumber = phoneNumber;
        
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task AddOrder(IOrder order)
    {
        _logger.LogInformation("Add order to customer");
        _orders.Add(order);
        order.SetCustomer(this);
        return Task.CompletedTask;
    }
}