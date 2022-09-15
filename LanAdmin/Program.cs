using LanAdmin;

//IHost host = Host.CreateDefaultBuilder(args)
//    .ConfigureServices(services =>
//    {
//        services.AddHostedService<Worker>();
//    })
//    .Build();
//
//await host.RunAsync();

PackageManager.PopulatePackageList();
PackageManager.packageList[0].CLI.ResponderAction(args);
