using RaspberryPi.Interop;
using RaspberryPi.LibVncServer.Interop;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibVncServer
{
    public sealed class RfbScreen : IDisposable
    {
        private IntPtr m_ptr;
        private readonly InteropHandler<rfbScreenInfo> m_handle;

        internal InteropHandler<rfbScreenInfo> Handle => this.m_handle;

        //internal IntPtr ScaledScreenNext
        //{
        //    get { return this.m_handle.ReadIntPtr(); }
        //    set { this.m_handle.WriteIntPtr(value); }
        //}

        //internal int ScaledScreenRefCount
        //{
        //    get { return this.m_handle.ReadInt32(); }
        //    set { this.m_handle.WriteInt32(value); }
        //}

        //public int Width
        //{
        //    get { return this.m_handle.ReadInt32(); }
        //    set { this.m_handle.WriteInt32(value); }
        //}
        //[FieldIndex]
        //public int PaddedWidthInBytes
        //{
        //    get { return this.m_handle.ReadInt32(); }
        //    set { this.m_handle.WriteInt32(value); }
        //}
        //[FieldIndex]
        //public int Height
        //{
        //    get { return this.m_handle.ReadInt32(); }
        //    set { this.m_handle.WriteInt32(value); }
        //}
        //[FieldIndex]
        //public int Depth
        //{
        //    get { return this.m_handle.ReadInt32(); }
        //    set { this.m_handle.WriteInt32(value); }
        //}
        //[FieldIndex]
        //public int BitsPerPixel
        //{
        //    get { return this.m_handle.ReadInt32(); }
        //    set { this.m_handle.WriteInt32(value); }
        //}
        //[FieldIndex]
        //public int SizeInBytes
        //{
        //    get { return this.m_handle.ReadInt32(); }
        //    set { this.m_handle.WriteInt32(value); }
        //}
        //[FieldIndex]
        //public uint BlackPixel
        //{
        //    get { return this.m_handle.ReadUInt32(); }
        //    set { this.m_handle.WriteUInt32(value); }
        //}
        //[FieldIndex]
        //public uint WhitePixel
        //{
        //    get { return this.m_handle.ReadUInt32(); }
        //    set { this.m_handle.WriteUInt32(value); }
        //}
        //[FieldIndex]
        //public IntPtr ScreenData
        //{
        //    get { return this.m_handle.ReadIntPtr(); }
        //    set { this.m_handle.WriteIntPtr(value); }
        //}
        //[FieldIndex]
        //public rfbPixelFormat ServerFormat
        //{
        //    get { return this.m_handle.ReadStruct<rfbPixelFormat>(); }
        //    set { this.m_handle.WriteStruct(value); }
        //}
        //[FieldIndex]
        //public rfbColourMap ColourMap
        //{
        //    get { return this.m_handle.ReadStruct<rfbColourMap>(); }
        //    set { this.m_handle.WriteStruct(value); }
        //}
        //[FieldIndex(StringMode.LPStr)]
        //public string DesktopName
        //{
        //    get { return this.m_handle.ReadString(); }
        //    set { this.m_handle.WriteString(value); }
        //}
        //[FieldIndex(StringMode.SizeConst, 255)]
        //public string ThisHost
        //{
        //    get { return this.m_handle.ReadString(); }
        //    set { this.m_handle.WriteString(value); }
        //}
        //[FieldIndex]
        //public bool AutoPort
        //{
        //    get { return this.m_handle.ReadBool(); }
        //    set { this.m_handle.WriteBool(value); }
        //}
        //[FieldIndex]
        //public int Port
        //{
        //    get { return this.m_handle.ReadInt32(); }
        //    set { this.m_handle.WriteInt32(value); }
        //}

        //[FieldIndex]
        //internal IntPtr ListenSock
        //{
        //    get { return this.m_handle.ReadIntPtr(); }
        //    set { this.m_handle.WriteIntPtr(value); }
        //}
        //[FieldIndex]
        //internal int MaxSock
        //{
        //    get { return this.m_handle.ReadInt32(); }
        //    set { this.m_handle.WriteInt32(value); }
        //}
        //[FieldIndex]
        //internal int MaxFd
        //{
        //    get { return this.m_handle.ReadInt32(); }
        //    set { this.m_handle.WriteInt32(value); }
        //}
        //[FieldIndex(int.MinValue, 128)]
        //internal IntPtr AllFds
        //{
        //    get { return this.m_handle.ReadIntPtr(); }
        //    set { this.m_handle.WriteIntPtr(value); }
        //}
        //[FieldIndex]
        //internal int SocketState
        //{
        //    get { return this.m_handle.ReadInt32(); }
        //    set { this.m_handle.WriteInt32(value); }
        //}
        //[FieldIndex]
        //internal IntPtr InetdSock
        //{
        //    get { return this.m_handle.ReadIntPtr(); }
        //    set { this.m_handle.WriteIntPtr(value); }
        //}
        //[FieldIndex]
        //internal bool InetdInitDone
        //{
        //    get { return this.m_handle.ReadBool(); }
        //    set { this.m_handle.WriteBool(value); }
        //}
        //[FieldIndex]
        //public int UdpPort
        //{
        //    get { return this.m_handle.ReadInt32(); }
        //    set { this.m_handle.WriteInt32(value); }
        //}
        //[FieldIndex]
        //internal IntPtr udpSock
        //{
        //    get { return this.m_handle.ReadIntPtr(); }
        //    set { this.m_handle.WriteIntPtr(value); }
        //}
        //[FieldIndex]
        //internal IntPtr UdpClient
        //{
        //    get { return this.m_handle.ReadIntPtr(); }
        //    set { this.m_handle.WriteIntPtr(value); }
        //}
        //[FieldIndex]
        //internal bool UdpSockConnected
        //{
        //    get { return this.m_handle.ReadBool(); }
        //    set { this.m_handle.WriteBool(value); }
        //}
        //[FieldIndex(int.MinValue, 16)]
        //internal IntPtr UdpRemoteAddr
        //{
        //    get { return this.m_handle.ReadIntPtr(); }
        //    set { this.m_handle.WriteIntPtr(value); }
        //}
        //[FieldIndex]
        //public int MaxClientWait
        //{
        //    get { return this.m_handle.ReadInt32(); }
        //    set { this.m_handle.WriteInt32(value); }
        //}
        //[FieldIndex]
        //internal bool HttpInitDone
        //{
        //    get { return this.m_handle.ReadBool(); }
        //    set { this.m_handle.WriteBool(value); }
        //}
        //[FieldIndex]
        //internal bool HttpEnableProxyConnect
        //{
        //    get { return this.m_handle.ReadBool(); }
        //    set { this.m_handle.WriteBool(value); }
        //}
        //[FieldIndex]
        //public int HttpPort
        //{
        //    get { return this.m_handle.ReadInt32(); }
        //    set { this.m_handle.WriteInt32(value); }
        //}
        //[FieldIndex(StringMode.LPStr)]
        //public string HttpDir
        //{
        //    get { return this.m_handle.ReadString(); }
        //    set { this.m_handle.WriteString(value); }
        //}
        //[FieldIndex]
        //internal IntPtr HttpListenSock
        //{
        //    get { return this.m_handle.ReadIntPtr(); }
        //    set { this.m_handle.WriteIntPtr(value); }
        //}
        //[FieldIndex]
        //internal IntPtr HttpSock
        //{
        //    get { return this.m_handle.ReadIntPtr(); }
        //    set { this.m_handle.WriteIntPtr(value); }
        //}
        //[FieldIndex]
        //internal IntPtr PasswordCheck
        //{
        //    get { return this.m_handle.ReadIntPtr(); }
        //    set { this.m_handle.WriteIntPtr(value); }
        //}
        //[FieldIndex]
        //internal IntPtr AuthPasswdData
        //{
        //    get { return this.m_handle.ReadIntPtr(); }
        //    set { this.m_handle.WriteIntPtr(value); }
        //}
        //[FieldIndex]
        //public int AuthPasswdFirstViewOnly
        //{
        //    get { return this.m_handle.ReadInt32(); }
        //    set { this.m_handle.WriteInt32(value); }
        //}
        //[FieldIndex]
        //public int MaxRectsPerUpdate
        //{
        //    get { return this.m_handle.ReadInt32(); }
        //    set { this.m_handle.WriteInt32(value); }
        //}
        //[FieldIndex]
        //public int DeferUpdateTime
        //{
        //    get { return this.m_handle.ReadInt32(); }
        //    set { this.m_handle.WriteInt32(value); }
        //}
        //[FieldIndex]
        //public bool AlwaysShared
        //{
        //    get { return this.m_handle.ReadBool(); }
        //    set { this.m_handle.WriteBool(value); }
        //}
        //[FieldIndex]
        //public bool NeverShared
        //{
        //    get { return this.m_handle.ReadBool(); }
        //    set { this.m_handle.WriteBool(value); }
        //}
        //[FieldIndex]
        //public bool DontDisconnect
        //{
        //    get { return this.m_handle.ReadBool(); }
        //    set { this.m_handle.WriteBool(value); }
        //}
        //[FieldIndex]
        //internal IntPtr ClientHead
        //{
        //    get { return this.m_handle.ReadIntPtr(); }
        //    set { this.m_handle.WriteIntPtr(value); }
        //}
        //[FieldIndex]
        //internal IntPtr PointerClient
        //{
        //    get { return this.m_handle.ReadIntPtr(); }
        //    set { this.m_handle.WriteIntPtr(value); }
        //}

        //public void DumpProps()
        //{
        //    foreach (var prop in this.GetType().GetTypeInfo().GetProperties())
        //    {
        //        var att = prop.GetCustomAttribute<FieldIndexAttribute>();
        //        if (att == null)
        //            continue;
        //        Console.WriteLine($"{prop.Name}={prop.GetValue(this)}");
        //    }
        //}

        //public void DumpPrintf()
        //{
        //    foreach (var prop in this.GetType().GetTypeInfo().GetProperties())
        //    {
        //        var att = prop.GetCustomAttribute<FieldIndexAttribute>();
        //        if (att == null)
        //            continue;
        //        string name = prop.Name.ToLowerInvariant().Substring(0, 1) + prop.Name.Substring(1);
        //        string format;
        //        if (prop.PropertyType == typeof(string))
        //            format = "%s";
        //        else if (prop.PropertyType == typeof(IntPtr))
        //            format = "%p";
        //        else
        //            format = "%d";
        //        Console.WriteLine($@"printf(""{name}={format}\n"", server->name);");
        //    }
        //}

        public RfbScreen(string[] args, int width, int height, int bitsPerSample, int samplesPerPixel, int bytesPerPixel)
        {
            int argc = 0;
            if (args != null)
                argc = args.Length;
            this.m_ptr = NativeMethods.rfbGetScreen(argc, args, width, height, bitsPerSample, samplesPerPixel, bytesPerPixel);
            if (this.m_ptr == IntPtr.Zero)
                throw new InvalidOperationException("RfbScreen allocation failed");
            this.m_handle = new InteropHandler<rfbScreenInfo>(() => this.m_ptr);
        }

        private bool disposedValue = false; // Pour détecter les appels redondants

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }
                NativeMethods.rfbScreenCleanup(this.m_ptr);

                disposedValue = true;
            }
        }

        ~RfbScreen()
        {
            // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
            Dispose(false);
        }

        // Ce code est ajouté pour implémenter correctement le modèle supprimable.
        public void Dispose()
        {
            // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public RfbServer InitServer()
        {
            return new RfbServer(this);
        }
    }
}
