using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace RaspberryPi.DisplaySpyServer
{
    internal sealed class SpyImageProvider : IScreenSpyImageProvider, IDisposable
    {
        private readonly ILogger<IScreenSpyImageProvider> m_logger;
        private readonly ImageGrabberService m_svc;
        private readonly BufferBlock<ImageData> m_buffer;
        private readonly IDisposable m_link;

        public SpyImageProvider(ILogger<IScreenSpyImageProvider> logger, ImageGrabberService grabberService)
        {
            this.m_logger = logger;
            this.m_svc = grabberService;

            this.m_buffer = new BufferBlock<ImageData>();
            this.m_link = this.m_svc.LinkOutputTo(this.m_buffer);
        }

        public void Dispose()
        {
            this.m_link.Dispose();
            this.m_buffer.Complete();
        }

        public async Task<ImageData> WaitOne(CancellationToken cancellationToken)
        {
            return await this.m_buffer.ReceiveAsync(cancellationToken);
        }
    }
}
