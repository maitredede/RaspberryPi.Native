using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibVncServer
{
    [StructLayout(LayoutKind.Sequential)]
    public sealed class fd_set
    {
        const int FD_SETSIZE = 64;

        public ushort fd_count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = FD_SETSIZE)]
        public IntPtr[] fd_array;
    }
}
