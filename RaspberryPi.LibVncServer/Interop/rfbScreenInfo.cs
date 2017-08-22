using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibVncServer
{
    [StructLayout(LayoutKind.Sequential)]
    public struct rfbScreenInfo
    {
        /** this structure has children that are scaled versions of this screen */
        //struct _rfbScreenInfo *scaledScreenNext;
        public IntPtr scaledScreenNext;
        public int scaledScreenRefCount;

        public int width;
        public int paddedWidthInBytes;
        public int height;
        public int depth;
        public int bitsPerPixel;
        public int sizeInBytes;

        public uint blackPixel;
        public uint whitePixel;

        /**
         * some screen specific data can be put into a struct where screenData
         * points to. You need this if you have more than one screen at the
         * same time while using the same functions.
         */
        //void* screenData;
        public IntPtr screenData;

        /* additions by libvncserver */

        public rfbPixelFormat serverFormat;
        public rfbColourMap colourMap; /**< set this if rfbServerFormat.trueColour==FALSE */
        [MarshalAs(UnmanagedType.LPStr)]
        public string desktopName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 254)]
        public string thisHost;

        public bool autoPort;
        public short port;
        //SOCKET listenSock;
        public IntPtr listenSock;
        public short maxSock;
        public short maxFd;
        public fd_set allFds;

        public rfbSocketState socketState;
        //SOCKET inetdSock;
        public IntPtr inetdSock;
        public bool inetdInitDone;

        public short udpPort;
        //SOCKET udpSock;
        public IntPtr udpSock;
        //struct _rfbClientRec* udpClient;
        public IntPtr udpClient;
        public bool udpSockConnected;
        //struct sockaddr_in udpRemoteAddr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2 + 4 + 4 + 8)]
        public byte[] udpRemoteAddr;

        public short maxClientWait;

        /* http stuff */
        public bool httpInitDone;
        public bool httpEnableProxyConnect;
        public short httpPort;
        //char* httpDir;
        [MarshalAs(UnmanagedType.LPStr)]
        public string httpDir;

        //SOCKET httpListenSock;
        public IntPtr httpListenSock;
        //SOCKET httpSock;
        public IntPtr httpSock;

        //rfbPasswordCheckProcPtr passwordCheck;
        public IntPtr passwordCheck;

        //void* authPasswdData;
        public IntPtr authPasswdData;
        /** If rfbAuthPasswdData is given a list, this is the first
            view only password. */
        public short authPasswdFirstViewOnly;

        /** send only this many rectangles in one update */
        public short maxRectsPerUpdate;
        /** this is the amount of milliseconds to wait at least before sending
         * an update. */
        public short deferUpdateTime;
        public bool alwaysShared;
        public bool neverShared;
        public bool dontDisconnect;
        //struct _rfbClientRec* clientHead;
        public IntPtr clientHead;
        //struct _rfbClientRec* pointerClient;  /**< "Mutex" for pointer events */
        public IntPtr pointerClient;  /**< "Mutex" for pointer events */

        /* cursor */
        public short cursorX, cursorY, underCursorBufferLen;
        //char* underCursorBuffer;
        public IntPtr underCursorBuffer;
        public bool dontConvertRichCursorToXCursor;
        //struct rfbCursor* cursor;
        public IntPtr cursor;

        /**
         * the frameBuffer has to be supplied by the serving process.
         * The buffer will not be freed by
         */
        //char* frameBuffer;
        public IntPtr frameBuffer;
        //rfbKbdAddEventProcPtr kbdAddEvent;
        public IntPtr kbdAddEvent;
        //rfbKbdReleaseAllKeysProcPtr kbdReleaseAllKeys;
        public IntPtr kbdReleaseAllKeys;
        //rfbPtrAddEventProcPtr ptrAddEvent;
        public IntPtr ptrAddEvent;
        //rfbSetXCutTextProcPtr setXCutText;
        public IntPtr setXCutText;
        //rfbGetCursorProcPtr getCursorPtr;
        public IntPtr getCursorPtr;
        //rfbSetTranslateFunctionProcPtr setTranslateFunction;
        public IntPtr setTranslateFunction;
        //rfbSetSingleWindowProcPtr setSingleWindow;
        public IntPtr setSingleWindow;
        //rfbSetServerInputProcPtr setServerInput;
        public IntPtr setServerInput;
        //rfbFileTransferPermitted getFileTransferPermission;
        public IntPtr getFileTransferPermission;
        //rfbSetTextChat setTextChat;
        public IntPtr setTextChat;

        /** newClientHook is called just after a new client is created */
        //rfbNewClientHookPtr newClientHook;
        public IntPtr newClientHook;
        /** displayHook is called just before a frame buffer update */
        //rfbDisplayHookPtr displayHook;
        public IntPtr displayHook;

        /** These hooks are called to pass keyboard state back to the client */
        //rfbGetKeyboardLedStateHookPtr getKeyboardLedStateHook;
        public IntPtr getKeyboardLedStateHook;

        /** if TRUE, an ignoring signal handler is installed for SIGPIPE */
        public bool ignoreSIGPIPE;

        /** if not zero, only a slice of this height is processed every time
         * an update should be sent. This should make working on a slow
         * link more interactive. */
        public short progressiveSliceHeight;

        //in_addr_t listenInterface;
        public uint listenInterface;
        public short deferPtrUpdateTime;

        /** handle as many input events as possible (default off) */
        public bool handleEventsEagerly;

        /** rfbEncodingServerIdentity */
        [MarshalAs(UnmanagedType.LPStr)]
        public string versionString;

        /** What does the server tell the new clients which version it supports */
        public short protocolMajorVersion;
        public short protocolMinorVersion;

        /** command line authorization of file transfers */
        public bool permitFileTransfer;

        /** displayFinishedHook is called just after a frame buffer update */
        //rfbDisplayFinishedHookPtr displayFinishedHook;
        public IntPtr displayFinishedHook;
        /** xvpHook is called to handle an xvp client message */
        //rfbXvpHookPtr xvpHook;
        public IntPtr xvpHook;

        public short ipv6port; /**< The port to listen on when using IPv6.  */
        //char* listen6Interface;
        public IntPtr listen6Interface;
        /* We have an additional IPv6 listen socket since there are systems that
           don't support dual binding sockets under *any* circumstances, for
           instance OpenBSD */
        //SOCKET listen6Sock;
        public IntPtr listen6Sock;
        public short http6Port;
        //SOCKET httpListen6Sock;
        public IntPtr httpListen6Sock;
    }
}
