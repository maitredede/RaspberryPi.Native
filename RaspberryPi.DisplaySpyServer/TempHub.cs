using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaspberryPi.DisplaySpyServer
{
    public class TempHub : Hub<ITempHubClient>, ITempHubServer
    {
        private readonly ILogger<TempHub> m_logger;

        public TempHub(ILogger<TempHub> logger)
        {
            this.m_logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
