using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Repro.BugStruct
{
    public sealed class GenericCtor<T>
    {
       public GenericCtor()
        {
            Console.WriteLine($"Size in generic class ctor is {Marshal.SizeOf<T>()}");
        }
    }
}
