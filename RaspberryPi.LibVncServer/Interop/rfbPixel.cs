using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibVncServer
{
    [StructLayout(LayoutKind.Sequential)]
    public struct rfbPixel
    {
        public uint value;
    }
}
