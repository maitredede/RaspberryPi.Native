//using RaspberryPi.Native;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace RaspberryPi.MMAL
//{
//    public abstract class ParameterBase
//    {
//        private readonly HeaderClass m_header;

//        public HeaderClass Header => this.m_header;

//        internal ParameterBase(MMALParameterId id)
//        {
//            this.m_header = new HeaderClass(this, id);
//        }

//        public sealed class HeaderClass
//        {
//            private readonly ParameterBase m_param;

//            internal HeaderClass(ParameterBase param, MMALParameterId id)
//            {
//                this.m_param = param;
//                this.Id = id;
//            }

//            public MMALParameterId Id
//            {
//                get { return (MMALParameterId)this.m_param.HeaderStruct.id; }
//                private set { this.m_param.HeaderStruct.id = (uint)value; }
//            }
//        }

//        internal abstract MMAL_PARAMETER_HEADER_T HeaderStruct { get; }

//        internal abstract void UpdateSize();
//    }
//}
