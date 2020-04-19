using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CommunicaptionBackend.Api {

    public class TrainTriggerService : IHostedService, IDisposable {

        private static readonly TimeSpan period = TimeSpan.FromDays(1);

        public IServiceProvider Services { get; }

        private Timer timer;

        public TrainTriggerService(IServiceProvider services) {
            Services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken) {
            timer = new Timer(DoWork, null, period, period);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose() {
            timer?.Dispose();
        }

        private void DoWork(object state) {
            using (var scope = Services.CreateScope()) {
                var mainService = scope.ServiceProvider .GetRequiredService<MainService>();
                mainService.TriggerTrain();
            }
        }
    }
}
