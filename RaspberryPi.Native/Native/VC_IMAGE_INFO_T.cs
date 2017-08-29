using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public sealed class VC_IMAGE_INFO_T
    {
        public VC_IMAGE_YUVINFO_T yuv;
        public ushort info;
    }
}
