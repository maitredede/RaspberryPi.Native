using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public sealed class DISPMANX_PROTECTION_T
    {
        public IntPtr Handle;
    }
}
