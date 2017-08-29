using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Repro.BugStruct
{
    public sealed class Generic<T>
    {
        public void PrintSize()
        {
            Console.WriteLine($"Size in generic class method is {Marshal.SizeOf<T>()}");
        }
    }
}
