using API.Services;

namespace API.BackgroundServices;

public class MigrationBackgroundService : BackgroundService
{
    private readonly IServiceProvider _provider;

    public MigrationBackgroundService(IServiceProvider provider)
    {
        _provider = provider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _provider.CreateScope();
            var migrationService = scope.ServiceProvider.GetRequiredService<MigrationUpdateService>();

            await migrationService.ApplyMigrationsToAllTenantsAsync();
            await Task.Delay(TimeSpan.FromHours(6), stoppingToken); // Run every 6 hours
        }
    }
}
