using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Repro.BugStruct
{
    internal abstract class BaseGeneric<T>
    {
        private readonly int m_size;

        public BaseGeneric()
        {
            this.m_size = Marshal.SizeOf<T>();
        }

        public int Size => this.m_size;
    }
}
