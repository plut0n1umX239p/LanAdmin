using LanAdmin;

//IHost host = Host.CreateDefaultBuilder(args)
//    .ConfigureServices(_services =>
//    {
//        _services.AddHostedService<PackageWorker>();
//    })
//    .Build();
//
//await host.RunAsync();

PackageLoader.LoadPackages(null, new());
Console.ReadKey();
