using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public sealed class VCHI_MEM_HANDLE_T
    {
        public IntPtr handle;
    }
}
