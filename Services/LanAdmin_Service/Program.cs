//using LanAdmin_Service;
//
//IHost host = Host.CreateDefaultBuilder(args)
//    .ConfigureServices(services =>
//    {
//        services.AddHostedService<Worker>();
//    })
//    .Build();
//
//await host.RunAsync();
using LanAdmin.PackageManager;

PackageManager.GetFreatures();
foreach (var item in PackageManager.windowsFeature)
{
    Console.WriteLine($"{item.name}:{item.description}");
}

Console.ReadKey();