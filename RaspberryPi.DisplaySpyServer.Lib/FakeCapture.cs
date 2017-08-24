using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RaspberryPi.DisplaySpyServer
{
    internal sealed class FakeCapture : IScreenCapture
    {
        private readonly byte[] m_img;

        public FakeCapture()
        {
            using (MemoryStream ms = new MemoryStream())
            using (var res = typeof(FakeCapture).Assembly.GetManifestResourceStream("RaspberryPi.DisplaySpyServer.Resources.HDRI_Sample_Scene_Balls.jpg"))
            {
                res.CopyTo(ms);
                this.m_img = ms.ToArray();
            }
        }

        ImageData IScreenCapture.CaptureImage()
        {
            return new ImageData
            {
                Date = DateTime.Now,
                ImageJpeg = this.m_img
            };
        }
    }
}
