using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibVncServer.Interop
{
    internal sealed class MemoryReader
    {
        private readonly IntPtr m_ptr;
        private int m_offset;

        public int Offset => this.m_offset;

        public MemoryReader(IntPtr ptr) : this(ptr, 0) { }
        public MemoryReader(IntPtr ptr, int offset)
        {
            this.m_ptr = ptr;
            this.m_offset = offset;
        }

        private void Debug([CallerMemberName]string method = null)
        {
            Console.WriteLine($"Calling {method} off={this.m_offset}...");
        }

        internal void DumpHexHere(int count) => this.DumpHex(this.Offset, count);
        internal void DumpHex(int offset, int count)
        {
            Console.Write($"mem[@{offset}+{count}]:");
            for (int i = offset; i < offset + count; i++)
            {
                byte b = Marshal.ReadByte(this.m_ptr, i);
                Console.Write(" {0:X2}", b);
            }
            Console.WriteLine();
        }


        internal IntPtr ReadIntPtr()
        {
            this.Debug();

            IntPtr value = Marshal.ReadIntPtr(this.m_ptr, this.m_offset);
            this.m_offset += Marshal.SizeOf<IntPtr>();
            return value;
        }

        internal short ReadInt16()
        {
            this.Debug();
            short value = Marshal.ReadInt16(this.m_ptr, this.m_offset);
            this.m_offset += Marshal.SizeOf<short>();
            return value;
        }

        internal int ReadInt32()
        {
            this.Debug();
            int value = Marshal.ReadInt32(this.m_ptr, this.m_offset);
            this.m_offset += Marshal.SizeOf<int>();
            return value;
        }



        internal uint ReadUInt32()
        {
            this.Debug();
            uint value = unchecked((uint)Marshal.ReadInt32(this.m_ptr, this.m_offset));
            this.m_offset += Marshal.SizeOf<uint>();
            return value;
        }

        internal T ReadStruct<T>()
        {
            this.Debug();
            T value = Marshal.PtrToStructure<T>(this.m_ptr);
            int size = Marshal.SizeOf<T>();
            int padding = size % 8;
            int newSize = size + padding;
            Console.WriteLine("\tstruct size is {0}, padding is {1}, total {2}", size, padding, newSize);
            this.m_offset += size;
            return value;
        }

        internal string ReadStringPtr()
        {
            this.Debug();
            IntPtr value = Marshal.ReadIntPtr(this.m_ptr, this.m_offset);
            this.m_offset += Marshal.SizeOf<IntPtr>();
            Console.WriteLine($"Value is {value}");
            if (value == IntPtr.Zero)
                return null;
            return Marshal.PtrToStringAnsi(value);
        }

        internal void Skip(int count)
        {
            this.m_offset += count;
        }

        internal string ReadString(int length)
        {
            this.Debug();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                byte b = Marshal.ReadByte(this.m_ptr, this.m_offset + i);
                if (b == 0)
                    break;
                sb.Append((char)b);
            }
            this.m_offset += length;
            return sb.ToString();
        }

        internal bool ReadBool()
        {
            this.Debug();
            byte b = Marshal.ReadByte(this.m_ptr, this.m_offset);
            this.m_offset += Marshal.SizeOf<byte>();
            return b != 0;
        }
    }
}
