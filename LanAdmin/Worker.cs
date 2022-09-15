namespace LanAdmin;

public class PackageWorker : BackgroundService
{
    private readonly ILogger<Worker> logger;

    public PackageWorker(ILogger<Worker> _logger)
    {
        logger = _logger;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            PackageManager.PopulatePackageListFromFloder();
            PackageManager.packageList[0].CLI.ResponderAction(new string[0] { } );
            while (true);
        }
    }
}