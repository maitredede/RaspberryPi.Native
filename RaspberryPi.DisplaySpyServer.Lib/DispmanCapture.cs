using ImageSharp;
using ImageSharp.Drawing;
using ImageSharp.Drawing.Pens;
using ImageSharp.Drawing.Brushes;
using ImageSharp.Formats;
using SixLabors.Fonts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using static RaspberryPi.Utils;
using ImageSharp.PixelFormats;

namespace RaspberryPi.DisplaySpyServer
{
    internal sealed class DispmanCapture : IScreenCapture, IDisposable
    {
        private readonly BcmHost m_host;
        private readonly Display m_display;

        private readonly int m_pitch;
        private readonly byte[] m_image;
        private readonly Resource m_resource;
        private readonly Rectangle m_screenRect;

        public DispmanCapture(BcmHost host)
        {
            this.m_host = host;
            this.m_display = this.m_host.Dispman.DisplayOpen(Screen.MAIN_LCD);
            this.m_pitch = ALIGN_UP(2 * this.m_display.Width, 32);
            this.m_image = new byte[this.m_pitch * this.m_display.Height];
            this.m_resource = this.m_host.Dispman.CreateResource(VC_IMAGE_TYPE_T.VC_IMAGE_RGB565, this.m_display.Width, this.m_display.Height);
            this.m_screenRect = new Rectangle(0, 0, this.m_display.Width, this.m_display.Height);
        }

        public void Dispose()
        {
            this.m_resource.Dispose();
            this.m_display.Dispose();
        }

        ImageData IScreenCapture.CaptureImage()
        {
            this.m_display.Snapshot(this.m_resource, DISPMANX_TRANSFORM_T.DISPMANX_NO_ROTATE);
            this.m_resource.ReadData(this.m_screenRect, this.m_image, this.m_display.Width * 2);

            ImageData data;
            using (var img = Image.LoadPixelData<Bgr565>(new Span<byte>(this.m_image), this.m_screenRect.Width, this.m_screenRect.Height))
            using (MemoryStream ms = new MemoryStream())
            {
                img.SaveAsJpeg(ms);
                data = new ImageData
                {
                    Date = DateTime.Now,
                    ImageJpeg = ms.ToArray()
                };
            }
            return data;
        }
    }
}
