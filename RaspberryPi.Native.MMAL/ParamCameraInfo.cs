//using RaspberryPi.Native;
//using System;
//using System.Collections.Generic;
//using System.Runtime.InteropServices;
//using System.Text;

//namespace RaspberryPi.MMAL
//{
//    public sealed class ParamCameraInfo : ParameterBase
//    {
//        private readonly MMAL_PARAMETER_CAMERA_INFO_T m_info;

//        public ParamCameraInfo() : base(MMALParameterId.MMAL_PARAMETER_CAMERA_INFO)
//        {
//            this.m_info = new MMAL_PARAMETER_CAMERA_INFO_T();
//        }

//        internal override MMAL_PARAMETER_HEADER_T HeaderStruct { get { return this.m_info.hdr; } }

//        internal override void UpdateSize()
//        {
//            this.m_info.hdr.size = (uint)Marshal.SizeOf<MMAL_PARAMETER_CAMERA_INFO_T>();
//        }
//    }
//}
