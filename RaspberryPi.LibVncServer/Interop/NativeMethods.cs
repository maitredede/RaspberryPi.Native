using System;
using System.Runtime.InteropServices;

namespace RaspberryPi.LibVncServer.Interop
{
    internal static class NativeMethods
    {
        internal const string LIB = "libvncserver";
        //internal const string LIB = "vncserver";

        [DllImport(LIB, EntryPoint = "rfbGetScreen", CallingConvention = CallingConvention.Cdecl)]
        //public static extern IntPtr rgbGetScreen(int argc, string[] argv, int width, int height, int bitsPerSample, int samplesPerPixel, int bytesPerPixel);
        public static extern IntPtr rfbGetScreen(int argc, string[] argv, int width, int height, int bitsPerSample, int samplesPerPixel, int bytesPerPixel);

        [DllImport(LIB, EntryPoint = "rfbScreenCleanup", CallingConvention = CallingConvention.Cdecl)]
        public static extern void rfbScreenCleanup(IntPtr screen);

        public static void rfbInitServer(IntPtr rfbScreen)
        {
            try
            {
                rfbInitServerWithPthreadsAndZRLE(rfbScreen);
                return;
            }
            catch
            {
            }
            try
            {
                rfbInitServerWithPthreadsButWithoutZRLE(rfbScreen);
                return;
            }
            catch
            {
            }
            try
            {
                rfbInitServerWithoutPthreadsButWithZRLE(rfbScreen);
                return;
            }
            catch
            {
            }
            rfbInitServerWithoutPthreadsAndZRLE(rfbScreen);
        }

        [DllImport(LIB, EntryPoint = "rfbInitServerWithPthreadsAndZRLE", CallingConvention = CallingConvention.Cdecl)]
        public static extern void rfbInitServerWithPthreadsAndZRLE(IntPtr rfbScreen);
        [DllImport(LIB, EntryPoint = "rfbInitServerWithPthreadsButWithoutZRLE", CallingConvention = CallingConvention.Cdecl)]
        public static extern void rfbInitServerWithPthreadsButWithoutZRLE(IntPtr rfbScreen);
        [DllImport(LIB, EntryPoint = "rfbInitServerWithoutPthreadsButWithZRLE", CallingConvention = CallingConvention.Cdecl)]
        public static extern void rfbInitServerWithoutPthreadsButWithZRLE(IntPtr rfbScreen);
        [DllImport(LIB, EntryPoint = "rfbInitServerWithoutPthreadsAndZRLE", CallingConvention = CallingConvention.Cdecl)]
        public static extern void rfbInitServerWithoutPthreadsAndZRLE(IntPtr rfbScreen);

        [DllImport(LIB, EntryPoint = "rfbShutdownServer", CallingConvention = CallingConvention.Cdecl)]
        public static extern void rfbShutdownServer(IntPtr rfbScreen, bool disconnectClients);

        [DllImport(LIB, EntryPoint = "rfbIsActive", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool rfbIsActive(IntPtr rfbScreen);


        [DllImport(LIB, EntryPoint = "rfbMarkRectAsModified", CallingConvention = CallingConvention.Cdecl)]
        public static extern void rfbMarkRectAsModified(RawRfbScreenInfo rfbScreen, int x1, int y1, int x2, int y2);

        [DllImport(LIB, EntryPoint = "rfbProcessEvents", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool rfbProcessEvents(RawRfbScreenInfo screenInfo, int usec);
    }
}
