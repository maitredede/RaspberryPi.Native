using RaspberryPi.LibVncServer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static RaspberryPi.Utils;

namespace RaspberryPi.DispmanVncServer
{
    public sealed class DispmanVncServerDisplay : IDisposable
    {
        private readonly BcmHost m_host;
        private readonly Display m_display;
        private readonly System.Diagnostics.Stopwatch m_swatch;
        private IntPtr m_framebuffer;

        public DispmanVncServerDisplay(BcmHost host, Display display)
        {
            this.m_host = host;
            this.m_display = display;
            this.m_swatch = new System.Diagnostics.Stopwatch();
        }

        public void Dispose()
        {
            this.m_swatch.Stop();
            if (this.m_framebuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(this.m_framebuffer);
                this.m_framebuffer = IntPtr.Zero;
            }
        }

        private static readonly short BPP = 2;


        public void Run()
        {
            VC_IMAGE_TYPE_T type = VC_IMAGE_TYPE_T.VC_IMAGE_RGB565;

            short padded_width;
            short pitch;
            short r_x0, r_y0, r_x1, r_y1;
            byte[] image;
            byte[] back_image;

            pitch = ALIGN_UP((short)(2 * this.m_display.Width), 32);
            padded_width = (short)(pitch / BPP);

            image = new byte[pitch * this.m_display.Height];
            back_image = new byte[pitch * this.m_display.Height];

            r_x0 = r_y0 = 0;
            r_x1 = (short)this.m_display.Width;
            r_y1 = (short)this.m_display.Height;
            using (Resource resource = this.m_host.Dispman.CreateResource(type, this.m_display.Width, this.m_display.Height))
            {

                using (RfbScreen server = new RfbScreen(null, padded_width, this.m_display.Height, 5, 3, BPP))
                {
                    server.DumpProps();
                    server.DumpPrintf();

                    Console.WriteLine("width={0}", server.Width);
                    Console.WriteLine("paddedWidthInBytes={0}", server.PaddedWidthInBytes);
                    Console.WriteLine("height={0}", server.Height);
                    Console.WriteLine("depth={0}", server.Depth);
                    Console.WriteLine("bitsPerPixel={0}", server.BitsPerPixel);
                    Console.WriteLine("sizeInBytes={0}", server.SizeInBytes);
                    Console.WriteLine("blackPixel={0}", server.BlackPixel);
                    Console.WriteLine("whitePixel={0}", server.WhitePixel);
                    //Console.WriteLine("screenData={0}", server.screenData);
                    Console.WriteLine("desktopName={0}", server.DesktopName);
                    Console.WriteLine("thisHost={0}", server.ThisHost);
                    Console.WriteLine("autoPort={0}", server.AutoPort);
                    Console.WriteLine("port={0}", server.Port);
                    Console.WriteLine("udpPort={0}", server.UdpPort);
                    Console.WriteLine("deferUpdateTime={0}", server.DeferUpdateTime);

                    Console.WriteLine("====");
                    server.DesktopName = this.GetType().Name;
                    Console.WriteLine("desktopName={0}", server.DesktopName);
                    this.m_framebuffer = Marshal.AllocHGlobal(pitch * this.m_display.Height);
                    //server.FrameBuffer = this.m_framebuffer;
                    server.AlwaysShared = true;
                    Console.WriteLine("rfbInitServer");
                    using (var srv = server.InitServer())
                    {
                        Console.WriteLine("rfbInitServer ok");
                        while (srv.IsActive())
                        {
                            //if (TimeToTakePicture(server))
                            //{
                            //    if (TakePicture())
                            //    {
                            //        server.MarkRectAsModified(server, r_x0, r_y0, r_x1, r_y1);
                            //    }
                            //}
                            //usec = server.deferUpdateTime * 1000;
                            //server.ProcessEvents(server, usec);
                        }
                    }
                }

                //server.frameBuffer = this.m_framebuffer = Marshal.AllocHGlobal(pitch * this.m_display.Height);
                //server.alwaysShared = true;
                //int usec = 1000;

                //Console.WriteLine("rfbInitServer");
                //NativeMethods.rfbInitServer(server);
                //try
                //{

                //    Func<bool> TakePicture = () =>
                //    {
                //        var transform = DISPMANX_TRANSFORM_T.DISPMANX_NO_ROTATE;
                //        this.m_display.Snapshot(resource, transform);
                //        Rectangle rect = new Rectangle(0, 0, this.m_display.Width, this.m_display.Height);
                //        resource.ReadData(rect, image, (uint)pitch);

                //        //TODO : detect changed area

                //        return true;
                //    };

                //    while (NativeMethods.rfbIsActive(server))
                //    {
                //        if (TimeToTakePicture())
                //            if (TakePicture())

                //                NativeMethods.rfbMarkRectAsModified(server, r_x0, r_y0, r_x1, r_y1);

                //        usec = server.deferUpdateTime * 1000;
                //        NativeMethods.rfbProcessEvents(server, usec);
                //    }
                //}
                //finally
                //{
                //    Console.WriteLine("rfbShutdownServer");
                //    NativeMethods.rfbShutdownServer(server, true);
                //}
            }
        }

        private bool TimeToTakePicture()
        {
            if (this.m_swatch.Elapsed > TimeSpan.FromSeconds(1 / 60))
            {
                this.m_swatch.Restart();
                return true;
            }
            return false;
        }

        private static void DumpFields(object obj, int padding)
        {
            string pads = string.Empty.PadLeft(padding);
            if (obj == null)
            {
                Console.WriteLine($"{pads}null");
            }
            else
            {
                foreach (FieldInfo fi in obj.GetType().GetFields())
                {
                    object value = fi.GetValue(obj);
                    if (value == null)
                    {
                        Console.WriteLine($"{pads}{fi.Name}=null");
                    }
                    else
                    {
                        Console.WriteLine($"{pads}{fi.Name}={value}");
                        if (!value.GetType().GetTypeInfo().IsValueType && !(value is string))
                        {
                            DumpFields(value, padding + 2);
                        }
                    }
                }
            }
        }
    }
}
