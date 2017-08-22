using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.LibVncServer.Interop
{
    internal sealed class InteropHandlerData
    {
        public int Index { get; internal set; }
        public Type Type { get; internal set; }
        public int TypeSize { get; internal set; }
        public string Name { get; internal set; }
        public int Offset { get; internal set; }
        public StringMode StringMode { get; internal set; }
    }
}
