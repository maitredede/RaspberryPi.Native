//using System;

//namespace RaspberryPi.Interop
//{
//    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
//    internal sealed class FieldIndexAttribute : Attribute
//    {
//        public int Index { get; }
//        public StringMode StringMode { get; }
//        public int SizeConst { get; }

//        public FieldIndexAttribute(int index, int sizeConst) : this(index, StringMode.Invalid, sizeConst) { }
//        public FieldIndexAttribute(int index) : this(index, StringMode.Invalid, int.MinValue) { }
//        public FieldIndexAttribute() : this(int.MinValue, StringMode.Invalid, int.MinValue) { }
//        public FieldIndexAttribute(StringMode stringMode) : this(int.MinValue, stringMode, int.MinValue) { }
//        public FieldIndexAttribute(StringMode stringMode, int sizeConst) : this(int.MinValue, stringMode, sizeConst) { }
//        public FieldIndexAttribute(int index, StringMode stringMode, int sizeConst)
//        {
//            this.Index = index;
//            this.StringMode = stringMode;
//            this.SizeConst = sizeConst;
//        }
//    }
//}