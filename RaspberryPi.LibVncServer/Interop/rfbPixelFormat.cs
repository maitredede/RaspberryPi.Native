using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibVncServer
{
    [StructLayout(LayoutKind.Sequential)]
    public sealed class rfbPixelFormat
    {
        public byte bitsPerPixel;       /* 8,16,32 only */

        public byte depth;      /* 8 to 32 */

        public byte bigEndian;      /* True if multi-byte pixels are interpreted
				   as big endian, or if single-bit-per-pixel
				   has most significant bit of the byte
				   corresponding to first (leftmost) pixel. Of
				   course this is meaningless for 8 bits/pix */

        public byte trueColour;     /* If false then we need a "colour map" to
				   convert pixels to RGB.  If true, xxxMax and
				   xxxShift specify bits used for red, green
				   and blue */

        /* the following fields are only meaningful if trueColour is true */

        public ushort redMax;        /* maximum red value (= 2^n - 1 where n is the
				   number of bits used for red). Note this
				   value is always in big endian order. */

        public ushort greenMax;      /* similar for green */

        public ushort blueMax;       /* and blue */

        public byte redShift;       /* number of shifts needed to get the red
				   value in a pixel to the least significant
				   bit. To find the red value from a given
				   pixel, do the following:
				   1) Swap pixel value according to bigEndian
				      (e.g. if bigEndian is false and host byte
				      order is big endian, then swap).
				   2) Shift right by redShift.
				   3) AND with redMax (in host byte order).
				   4) You now have the red value between 0 and
				      redMax. */

        public byte greenShift;     /* similar for green */

        public byte blueShift;      /* and blue */

        public byte pad1;
        public ushort pad2;
    }
}
