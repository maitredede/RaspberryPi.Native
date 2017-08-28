using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace RaspberryPi.DisplaySpyServer
{
    public class ClockedService : IHostedService
    {
        private readonly ILogger<ClockedService> m_logger;
        private readonly IHubContext<TempHub> m_hub;
        private Timer m_timer;
        private CancellationTokenSource m_cts;

        public ClockedService(IHubContext<TempHub> hub, ILogger<ClockedService> logger)
        {
            this.m_logger = logger;
            this.m_hub = hub;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.m_timer = new Timer(this.Callback);
            this.m_cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            this.m_cts.Token.Register(this.m_timer.Dispose);
            this.m_timer.Change(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(3));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.m_cts.Cancel();
            return Task.CompletedTask;
        }

        private void Callback(object state)
        {
            ClockedData data = new ClockedData();
            try
            {
                data.CpuTemp = MetricsHelper.CpuTemp();
                data.GpuTemp = MetricsHelper.GpuTemp();
            }
            catch (Exception ex)
            {
                this.m_logger.LogError(ex, "Error creating clocked metrics");
            }
            data.Date = DateTime.Now;
            this.m_hub.Clients.All.InvokeAsync("data", data);
        }
    }
}
