using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace RaspberryPi.Camera.Sample.CameraDirect.Controllers
{
    [Produces("application/json")]
    [Route("camera")]
    public class CameraController : Controller
    {
        private readonly ICameraStillImageProvider m_still;
        private readonly ILogger<CameraController> m_logger;

        public CameraController(ICameraStillImageProvider stillProvider, ILogger<CameraController> logger)
        {
            this.m_still = stillProvider;
            this.m_logger = logger;
        }

        [Route("image")]
        public async Task<IActionResult> Image(CancellationToken cancellationToken)
        {
            this.m_logger.LogInformation("Getting stillimage");
            byte[] data = await this.m_still.CaptureImage();
            this.m_logger.LogInformation("Got stillimage");
            return this.File(data, "image/jpeg");
        }
    }
}