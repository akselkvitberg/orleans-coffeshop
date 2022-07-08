using Orleans;

namespace GrainInterfaces;

public interface IPhone : IGrainObserver
{
    Task Notify(string message);
    Task<string> PhoneNumber();
}