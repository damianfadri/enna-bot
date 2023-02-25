using Enna.Streamers.Application.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Enna.Streamers.Application
{
    public class WorkerService : IHostedService
    {
        private readonly IEnumerable<IWorker> _workers;
        private readonly WorkerOptions _options;

        public WorkerService(
            IEnumerable<IWorker> workers,
            IOptions<WorkerOptions> options)
        {
            _workers = workers;
            _options = options.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var timer = new PeriodicTimer(
                TimeSpan.FromMilliseconds(_options.PollingMs));

            while (await timer.WaitForNextTickAsync(cancellationToken))
            {
                foreach (var worker in _workers)
                {
                    await worker.DoWork();
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
