using Grains;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

try
{
    var builder = new SiloHostBuilder()
        .UseLocalhostClustering()
        .AddSimpleMessageStreamProvider("SMSProvider")
        .AddMemoryGrainStorage("PubSubStore")
        .ConfigureLogging(logging => logging.AddConsole())
        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(CoffeShop).Assembly).WithReferences());

    var host = builder.Build();
    await host.StartAsync();

    Console.WriteLine("\n\nPress Enter to terminate...\n\n");
    Console.ReadLine();

    await host.StopAsync();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}