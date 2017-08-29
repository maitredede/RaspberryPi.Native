﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Repro.BugStruct
{
    [StructLayout(LayoutKind.Sequential)]
    public sealed class MyClass
    {
        [MarshalAs(UnmanagedType.U4)]
        public int port_id;
        [MarshalAs(UnmanagedType.U4)]
        public int max_width;
        [MarshalAs(UnmanagedType.U4)]
        public int max_height;
        [MarshalAs(UnmanagedType.I4)]
        public int lens_present;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Program.MMAL_PARAMETER_CAMERA_INFO_MAX_STR_LEN)]
        public string camera_name;
    }
}
