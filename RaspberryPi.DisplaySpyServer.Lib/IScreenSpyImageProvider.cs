using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace RaspberryPi.DisplaySpyServer
{
    /// <summary>
    /// Interface to grab image
    /// </summary>
    public interface IScreenSpyImageProvider
    {
        /// <summary>
        /// Wait for next image
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ImageData> WaitOne(CancellationToken cancellationToken);
    }
}
