using Orleans;

namespace GrainInterfaces;

public interface ICustomer : IGrainWithStringKey
{
    Task<IOrder[]> Orders();

    Task NotifyOrderComplete(IOrder order);

    Task AddPhoneNumber(string phoneNumber);
    Task AddOrder(IOrder order);
}