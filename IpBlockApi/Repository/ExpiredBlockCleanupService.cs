
using IpBlockApi.Services;

namespace IpBlockApi.Repository
{
    public class ExpiredBlockCleanupService : BackgroundService
    {
        private readonly IServiceProvider _services;

        public ExpiredBlockCleanupService(IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _services.CreateScope())
                {
                    var temporalBlockService = scope.ServiceProvider.GetRequiredService<ITemporaryBlockService>();
                    temporalBlockService.RemoveExpiredBlocks();
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);   
            }
        }
    }
}

