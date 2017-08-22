using RaspberryPi.LibVncServer.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.LibVncServer
{
    public sealed class RfbServer : IDisposable
    {
        private readonly RfbScreen m_screen;

        internal RfbServer(RfbScreen screen)
        {
            this.m_screen = screen;
            NativeMethods.rfbInitServer(this.m_screen.Handle);
        }

        public bool IsActive()
        {
            return NativeMethods.rfbIsActive(this.m_screen.Handle);
        }

        public void Dispose()
        {
            NativeMethods.rfbShutdownServer(this.m_screen.Handle, true);
        }
    }
}
