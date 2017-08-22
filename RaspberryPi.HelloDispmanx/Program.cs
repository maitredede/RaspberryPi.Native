using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using static RaspberryPi.Utils;

namespace RaspberryPi.HelloDispmanx
{
    class Program
    {
        const short WIDTH = 200;
        const short HEIGHT = 200;

        static void Main(string[] args)
        {

            Alpha alpha = new Alpha
            {
                Flags = DISPMANX_FLAGS_ALPHA_T.DISPMANX_FLAGS_ALPHA_FROM_SOURCE | DISPMANX_FLAGS_ALPHA_T.DISPMANX_FLAGS_ALPHA_FIXED_ALL_PIXELS,
                Opacity = 120,
                Mask = null
            };
            Screen screen = Screen.MAIN_LCD;
            short width = WIDTH;
            short height = HEIGHT;
            short pitch = ALIGN_UP((short)(width * 2), 32);
            short aligned_height = ALIGN_UP(height, 16);
            VC_IMAGE_TYPE_T type = VC_IMAGE_TYPE_T.VC_IMAGE_RGB565;

            try
            {
                using (BcmHost host = new BcmHost())
                {
                    Console.WriteLine("Open display {0}", screen);
                    using (Display display = host.Dispman.DisplayOpen(screen))
                    {
                        Console.WriteLine("Display is: {0} {1}x{2}", display.DisplayNum, display.Width, display.Height);
                        byte[] image = new byte[pitch * height];
                        GCHandle imageHandle = GCHandle.Alloc(image, GCHandleType.Pinned);
                        try
                        {
                            FillRect(type, image, pitch, aligned_height, 0, 0, width, height, unchecked((short)0xFFFF));
                            FillRect(type, image, pitch, aligned_height, 0, 0, width, height, unchecked((short)0xF800));
                            FillRect(type, image, pitch, aligned_height, 20, 20, (short)(width - 40), (short)(height - 40), unchecked((short)0x07E0));
                            FillRect(type, image, pitch, aligned_height, 40, 40, (short)(width - 80), (short)(height - 80), unchecked((short)0x001F));
                        }
                        finally
                        {
                            imageHandle.Free();
                        }
                        using (Resource resource = host.Dispman.CreateResource(type, width, height))
                        {
                            Rectangle dst_rect = new Rectangle(0, 0, width, height);
                            resource.WriteData(pitch, image, dst_rect);

                            Update update = host.Dispman.UpdateStart(10);
                            Rectangle src_rect = new Rectangle(0, 0, width << 16, height << 16);
                            dst_rect = new Rectangle((display.Width - width) / 2, (display.Height - height) / 2, width, height);
                            Element element = host.Dispman.ElementAdd(update, display, 2000 /*layer*/ , dst_rect, resource, src_rect, Protection.None, alpha, null/*clamp*/, DISPMANX_TRANSFORM_T.DISPMANX_NO_ROTATE);

                            update.SubmitSync();

                            Console.WriteLine("Sleeping for 10 seconds...");
                            Thread.Sleep(TimeSpan.FromSeconds(10));

                            update = host.Dispman.UpdateStart(10);
                            host.Dispman.ElementRemove(update, element);
                            update.SubmitSync();
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName + ": " + ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private static void FillRect(VC_IMAGE_TYPE_T type, byte[] image, short pitch, short aligned_height, short x, short y, short w, short h, short val)
        {
            short row;
            short col;

            int linePos = y * (pitch >> 1) + x;
            byte[] bval = BitConverter.GetBytes(val);

            for (row = 0; row < h; row++)
            {
                for (col = 0; col < w; col++)
                {
                    Array.Copy(bval, 0, image, linePos + col, bval.Length);
                }
                linePos += (pitch >> 1);
            }
        }
    }
}