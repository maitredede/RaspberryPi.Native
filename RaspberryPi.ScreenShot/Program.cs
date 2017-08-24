using ImageSharp;
using ImageSharp.PixelFormats;
using System;
using System.Drawing;
using System.IO;
using static RaspberryPi.Utils;

namespace RaspberryPi.ScreenShot
{
    class Program
    {
        static void Main(string[] args)
        {
            using (BcmHost host = new BcmHost())
            using (Display disp = host.Dispman.DisplayOpen(Screen.MAIN_LCD))
            {
                int pitch = ALIGN_UP(2 * disp.Width, 32);
                VC_IMAGE_TYPE_T type = VC_IMAGE_TYPE_T.VC_IMAGE_RGB565;
                using (Resource res = host.Dispman.CreateResource(type, disp.Width, disp.Height))
                {

                    Console.WriteLine("snapshot");
                    disp.Snapshot(res, DISPMANX_TRANSFORM_T.DISPMANX_NO_ROTATE);

                    Console.WriteLine("readdata");
                    Rectangle rect = new Rectangle(0, 0, disp.Width, disp.Height);
                    byte[] image = new byte[pitch * disp.Height];
                    res.ReadData(rect, image, disp.Width * 2);

                    Console.WriteLine("jpg");
                    using (var img = Image.LoadPixelData<Bgr565>(new Span<byte>(image), rect.Width, rect.Height))
                    using (MemoryStream ms = new MemoryStream())
                    {
                        img.SaveAsJpeg(ms);
                    }

                    Console.WriteLine("ok");
                }
            }
        }
    }
}
