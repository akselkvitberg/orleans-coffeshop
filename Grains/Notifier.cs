using GrainInterfaces;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Concurrency;

namespace Grains;

[StatelessWorker]
public class Notifier : Grain, INotifier
{
    private ILogger<Notifier> _logger;
    private List<IPhone> _phones = new();

    public Notifier(ILogger<Notifier> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task Notify(string phoneNumber, string message)
    {
        _logger.LogInformation("Notify {PhoneNumber} with {Message}", phoneNumber, message);

        foreach (var phone in _phones)
        {
            var phonePhoneNumber = await phone.PhoneNumber();
            if (phonePhoneNumber == phoneNumber)
                await phone.Notify(message);
        }
    }

    /// <inheritdoc />
    public Task AddSubscriber(IPhone phone)
    {
        _logger.LogInformation("New phone - who dis?");
        _phones.Add(phone);
        return Task.CompletedTask;
    }
}