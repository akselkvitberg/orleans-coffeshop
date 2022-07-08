using Orleans;

namespace GrainInterfaces;

public interface INotifier : IGrainWithIntegerKey
{
    Task Notify(string phoneNumber, string message);

    Task AddSubscriber(IPhone phone);
}