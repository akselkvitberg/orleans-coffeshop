using GrainInterfaces;
using GrainInterfaces.Events;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Streams;

namespace Grains;

// [ImplicitStreamSubscription("AddOrder")]
public class Barista : Grain, IBarista
{
    private ILogger<Barista> _logger;
    private ICoffeShop _coffeShop;
    private IAsyncStream<ProductRequestEvent> _stream;

    public Barista(ILogger<Barista> logger)
    {
        _logger = logger;
    }

    public async Task OnNextAsync(ProductRequestEvent request, StreamSequenceToken _)
    {
        _logger.LogInformation("Barista making coffe");
        var coffeMachine = await _coffeShop.UseCoffeMachine();

        var product = await coffeMachine.MakeCoffee(request.Item);

        var order = GrainFactory.GetGrain<IOrder>(request.OrderId);
        await order.Fulfill(product);
    }

    /// <inheritdoc />
    public async Task StartWorking(ICoffeShop coffeShop)
    {
        _logger.LogInformation("Barista starting work at {CoffeShop}", coffeShop.GetPrimaryKeyString());
        _coffeShop = coffeShop;

        _stream = GetStreamProvider("SMSProvider")
            .GetStream<ProductRequestEvent>(_coffeShop.GetPrimaryKey(), "AddOrder");

        await _stream
            .SubscribeAsync(OnNextAsync);

    }
}