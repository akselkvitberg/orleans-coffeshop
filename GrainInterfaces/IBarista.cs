using Orleans;

namespace GrainInterfaces;

public interface IBarista : IGrainWithGuidKey
{
    Task StartWorking(ICoffeShop coffeShop);
}