using GrainInterfaces;
using Microsoft.Extensions.Logging;
using Orleans;

try
{
    await using var client = new ClientBuilder()
        .UseLocalhostClustering()
        .ConfigureLogging(logging => logging.AddConsole())
        .Build();

    await client.Connect();
    Console.WriteLine("Client successfully connected to silo host \n");

    // example of calling grains from the initialized client
    var customer = client.GetGrain<ICustomer>("Aksel Kvitberg");
    await customer.AddPhoneNumber("12345");

    var coffeShop = client.GetGrain<ICoffeShop>(Guid.NewGuid());
    await coffeShop.OpenShop();

    var menu = await coffeShop.GetMenu();
    foreach (var menuItem in menu.Items)
    {
        Console.WriteLine($"{menuItem.Name}\t{menuItem.Price}");
    }

    var order = client.GetGrain<IOrder>(Guid.NewGuid());
    await customer.AddOrder(order);

    await order.AddItem(menu.Items[0].Name);
    
    var phone = new Phone()
    {
        PhoneNumber = ("12345")
    };

    phone.OrderReady += async () =>
    {
        await coffeShop.GetProducts(order);
        Console.WriteLine("Got my coffee :)");
    };

    var notifier = client.GetGrain<INotifier>(0);
    var obj = await client.CreateObjectReference<IPhone>(phone);

    await notifier.AddSubscriber(obj);
    
    await coffeShop.AddOrder(order);

    Console.ReadKey();

}
catch (Exception e)
{
    Console.WriteLine($"\nException while trying to run client: {e.Message}");
    Console.WriteLine("Make sure the silo the client is trying to connect to is running.");
    Console.WriteLine("\nPress any key to exit.");
    Console.ReadKey();
}

public class Phone : IPhone
{
    public event Action? OrderReady;
    /// <inheritdoc />
    public Task Notify(string message)
    {
        Console.WriteLine("Your order is ready");
        OnOrderReady();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    Task<string> IPhone.PhoneNumber()
    {
        return Task.FromResult(PhoneNumber);
    }

    public string PhoneNumber { get; set; } = "";

    protected virtual void OnOrderReady()
    {
        OrderReady?.Invoke();
    }
}