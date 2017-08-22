using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace RaspberryPi.LibVncServer.Interop
{
    internal sealed class InteropHandler<T> : IDisposable
    {
        private readonly InteropHandlerData[] s_data;
        private readonly Dictionary<string, InteropHandlerData> s_offsets;

        private readonly IntPtr m_ptr;
        private readonly List<IntPtr> m_allocatedPointers = new List<IntPtr>();

        public IntPtr Ptr => this.m_ptr;

        public void Dispose()
        {
            while (this.m_allocatedPointers.Count > 0)
            {
                IntPtr ptr = this.m_allocatedPointers[0];
                Marshal.FreeHGlobal(ptr);
                this.m_allocatedPointers.RemoveAt(0);
            }
        }

        public InteropHandler(IntPtr ptr)
        {
            this.m_ptr = ptr;

            var lst = new List<InteropHandlerData>();
            foreach (PropertyInfo prop in typeof(T).GetTypeInfo().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                FieldIndexAttribute index = prop.GetCustomAttribute<FieldIndexAttribute>(true);
                if (index == null)
                    continue;
                InteropHandlerData data = new InteropHandlerData
                {
                    StringMode = index.StringMode,
                    Type = prop.PropertyType,
                    Name = prop.Name,
                };

                if (index.Index == int.MinValue)
                {
                    data.Index = lst.Count;
                }
                else
                {
                    data.Index = index.Index;
                }

                if (prop.PropertyType == typeof(string))
                {
                    switch (index.StringMode)
                    {
                        case StringMode.LPStr:
                            data.TypeSize = Marshal.SizeOf<IntPtr>();
                            break;
                        case StringMode.SizeConst:
                            data.TypeSize = index.SizeConst;
                            break;
                        case StringMode.Invalid:
                        default:
                            throw new ArgumentException($"Specify StringMode for property {prop.Name} on {typeof(T).FullName}");
                    }
                }
                else
                {
                    if (index.SizeConst == int.MinValue)
                    {
                        if (prop.PropertyType == typeof(bool))
                        {
                            data.TypeSize = Marshal.SizeOf<byte>();
                        }
                        else
                        {
                            data.TypeSize = (int)typeof(Marshal).GetTypeInfo().GetMethod(nameof(Marshal.SizeOf), Type.EmptyTypes).MakeGenericMethod(prop.PropertyType).Invoke(null, null);
                        }
                    }
                    else
                    {
                        data.TypeSize = index.SizeConst;
                    }
                }

                lst.Add(data);
            }
            lst.Sort((x, y) => x.Index.CompareTo(y.Index));
            s_data = lst.ToArray();
            for (int i = 0; i < s_data.Length; i++)
            {
                if (s_data[i].Index != i)
                    throw new InvalidOperationException($"Bad field index on {typeof(T).FullName} for theorical index {i}");

                int offset = 0;
                if (i > 0)
                {
                    offset = s_data[i - 1].Offset + s_data[i - 1].TypeSize;
                }
                s_data[i].Offset = offset;
            }
            s_offsets = s_data.ToDictionary(d => d.Name);
        }

        public void Dump()
        {
            Console.WriteLine($"== Accessors for {typeof(T).FullName} ({s_data.Length} fields)");
            for (int i = 0; i < this.s_data.Length; i++)
            {
                var d = this.s_data[i];
                Console.WriteLine($"\t{d.Name} @{d.Offset} s{d.TypeSize} t{d.Type.Name}");
            }
        }

        internal string ReadString([CallerMemberName]string prop = null)
        {
            InteropHandlerData data = this.s_offsets[prop];
            switch (data.StringMode)
            {
                case StringMode.LPStr:
                    IntPtr ptr = this.ReadIntPtr(prop);
                    return Marshal.PtrToStringAnsi(ptr);
                case StringMode.SizeConst:
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < data.TypeSize; i++)
                    {
                        byte b = Marshal.ReadByte(this.m_ptr, data.Offset + i);
                        if (b == 0)
                            break;
                        sb.Append((char)b);
                    }
                    return sb.ToString();
                default:
                    throw new InvalidOperationException("Invalid stringmode for property " + prop);
            }
        }

        internal void DumpHex(string property, int offset, int count)
        {
            InteropHandlerData data = this.s_offsets[property];
            int min = data.Offset + offset;
            int max = min + count;
            Console.WriteLine($"@{property} ({data.Offset}) from {offset} count {count}");
            for (int i = min; i < max; i++)
            {
                byte b = Marshal.ReadByte(this.m_ptr, i);
                Console.Write("{0:X2} ", b);
            }
            Console.WriteLine();
        }

        internal void WriteString(string value, [CallerMemberName]string prop = null)
        {
            InteropHandlerData data = this.s_offsets[prop];
            switch (data.StringMode)
            {
                case StringMode.LPStr:
                    if (value == null)
                    {
                        this.WriteIntPtr(IntPtr.Zero, prop);
                        return;
                    }
                    IntPtr ptr = Marshal.StringToHGlobalAnsi(value);
                    this.m_allocatedPointers.Add(ptr);
                    Marshal.WriteIntPtr(this.m_ptr, data.Offset, ptr);
                    break;
                case StringMode.SizeConst:
                    for (int i = 0; i < data.TypeSize; i++)
                    {
                        Marshal.WriteByte(this.m_ptr, data.Offset + i, 0);
                    }
                    if (string.IsNullOrEmpty(value))
                        return;
                    byte[] b = Encoding.UTF8.GetBytes(value);
                    for (int i = 0; i < b.Length; i++)
                    {
                        Marshal.WriteByte(this.m_ptr, data.Offset + i, b[i]);
                    }
                    break;
                default:
                    throw new InvalidOperationException("Invalid stringmode for property " + prop);
            }
        }

        internal TStruct ReadStruct<TStruct>([CallerMemberName]string prop = null)
        {
            InteropHandlerData data = this.s_offsets[prop];
            IntPtr ptr = this.m_ptr + data.Offset;
            return Marshal.PtrToStructure<TStruct>(ptr);
        }

        internal void WriteStruct<TStruct>(TStruct value, [CallerMemberName]string prop = null)
        {
            InteropHandlerData data = this.s_offsets[prop];
            IntPtr ptr = this.m_ptr + data.Offset;
            Marshal.StructureToPtr(value, ptr, false);
        }

        internal void WriteIntPtr(IntPtr value, [CallerMemberName]string prop = null)
        {
            InteropHandlerData data = this.s_offsets[prop];
            Marshal.WriteIntPtr(this.m_ptr, data.Offset, value);
        }

        internal IntPtr ReadIntPtr([CallerMemberName]string prop = null)
        {
            InteropHandlerData data = this.s_offsets[prop];
            return Marshal.ReadIntPtr(this.m_ptr, data.Offset);
        }

        internal void WriteInt32(int value, [CallerMemberName]string prop = null)
        {
            InteropHandlerData data = this.s_offsets[prop];
            Marshal.WriteInt32(this.m_ptr, data.Offset, value);
        }

        internal int ReadInt32([CallerMemberName]string prop = null)
        {
            InteropHandlerData data = this.s_offsets[prop];
            return Marshal.ReadInt32(this.m_ptr, data.Offset);
        }

        internal void WriteUInt32(uint value, [CallerMemberName]string prop = null)
        {
            InteropHandlerData data = this.s_offsets[prop];
            Marshal.WriteInt32(this.m_ptr, data.Offset, unchecked((int)value));
        }

        internal uint ReadUInt32([CallerMemberName]string prop = null)
        {
            InteropHandlerData data = this.s_offsets[prop];
            return unchecked((uint)Marshal.ReadInt32(this.m_ptr, data.Offset));
        }

        internal void WriteBool(bool value, [CallerMemberName]string prop = null)
        {
            InteropHandlerData data = this.s_offsets[prop];
            Marshal.WriteByte(this.m_ptr, data.Offset, value ? (byte)1 : (byte)0);
        }

        internal bool ReadBool([CallerMemberName]string prop = null)
        {
            InteropHandlerData data = this.s_offsets[prop];
            return Marshal.ReadByte(this.m_ptr, data.Offset) != 0;
        }
    }
}
