using System;
using System.Collections.Generic;
using System.Text;

namespace Repro.BugStruct
{
    internal sealed class HeritedGenericClass : BaseGeneric<MyClass>
    {
    }

    internal sealed class HeritedGenericStruct : BaseGeneric<MyStruct>
    {
    }
}
