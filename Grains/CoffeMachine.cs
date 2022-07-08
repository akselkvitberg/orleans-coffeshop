using GrainInterfaces;
using GrainInterfaces.Model;
using Microsoft.Extensions.Logging;
using Orleans;

namespace Grains;

public class CoffeMachine : Grain, ICoffeMachine
{
    private ILogger<CoffeMachine> _logger;

    public CoffeMachine(ILogger<CoffeMachine> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<Product> MakeCoffee(OrderItem orderItem)
    {
        _logger.LogInformation("Coffe machine making coffe");
        await Task.Delay(20);
        _logger.LogInformation("Coffe is ready");

        return new Product()
        {
            Name = orderItem.Name,
        };
    }
}