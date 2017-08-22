using System;
using System.Collections.Generic;
using System.Text;
using RaspberryPi.Native;
using System.Runtime.InteropServices;

namespace RaspberryPi.MMAL
{
    public sealed class CameraComponent : MMALComponent
    {
        internal CameraComponent(MMAL_COMPONENT_T handle, MMALComponentName name) : base(handle, name)
        {
        }
    }
}
