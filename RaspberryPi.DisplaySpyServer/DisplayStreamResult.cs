using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RaspberryPi.DisplaySpyServer
{
    public sealed class DisplayStreamResult : IActionResult
    {
        private readonly IScreenSpyImageProvider m_imageProvider;
        private readonly CancellationToken m_cancellationToken;

        private static readonly string CRLF = "\r\n";

        public DisplayStreamResult(IScreenSpyImageProvider imageProvider, CancellationToken cancellationToken)
        {
            this.m_imageProvider = imageProvider;
            this.m_cancellationToken = cancellationToken;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            string boundary = "--boundary-" + Guid.NewGuid().ToString() + "--";
            byte[] boundaryBytes = Encoding.UTF8.GetBytes(boundary + CRLF);

            var response = context.HttpContext.Response;
            response.StatusCode = 200;
            var headers = response.GetTypedHeaders();
            headers.CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
            {
                NoCache = true
            };
            headers.ContentType = new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("multipart/x-mixed-replace")
            {
                Boundary = boundary
            };
            try
            {
                while (!this.m_cancellationToken.IsCancellationRequested)
                {
                    ImageData data = await this.m_imageProvider.WaitOne(this.m_cancellationToken);
                    StringBuilder sb = new StringBuilder(boundary);
                    sb.Append(CRLF);
                    sb.Append("Content-Type: image/jpeg");
                    sb.Append(CRLF);
                    sb.AppendFormat("Content-Length: {0}", data.ImageJpeg.Length);
                    sb.Append(CRLF);
                    sb.Append(CRLF);
                    byte[] partHeaders = Encoding.UTF8.GetBytes(sb.ToString());
                    await response.Body.WriteAsync(partHeaders, 0, partHeaders.Length);
                    await response.Body.WriteAsync(data.ImageJpeg, 0, data.ImageJpeg.Length, this.m_cancellationToken);
                    await response.Body.FlushAsync(this.m_cancellationToken);
                }
            }
            catch(TaskCanceledException)
            {
            }
        }
    }
}
