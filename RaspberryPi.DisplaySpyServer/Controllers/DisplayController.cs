using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RaspberryPi.DisplaySpyServer.Controllers
{
    [Route("display")]
    public class DisplayController : Controller
    {
        private readonly IScreenSpyImageProvider m_imageProvider;

        public DisplayController(IScreenSpyImageProvider imageProvider)
        {
            this.m_imageProvider = imageProvider;
        }

        [Route("stream")]
        public IActionResult GetStream(CancellationToken cancellationToken)
        {
            return new DisplayStreamResult(this.m_imageProvider, cancellationToken);
        }

        [Route("image")]
        public async Task<IActionResult> GetImage(CancellationToken cancellationToken)
        {
            ImageData data = await this.m_imageProvider.WaitOne(cancellationToken);
            return this.File(data.ImageJpeg, "image/jpeg");
        }
    }
}