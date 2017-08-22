using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibVncServer
{
    [StructLayout(LayoutKind.Sequential)]
    public sealed class rfbColourMap
    {
        public uint count;
        public bool is16;
        public IntPtr data;
    }
}
