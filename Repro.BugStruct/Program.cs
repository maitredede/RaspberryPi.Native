using RaspberryPi;
using RaspberryPi.MMAL;
using RaspberryPi.MMAL.Interop;
using RaspberryPi.Native;
using System;
using System.Runtime.InteropServices;

namespace Repro.BugStruct
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Hello World! Size of MMAL_PARAMETER_STEREOSCOPIC_MODE_T is {Marshal.SizeOf<MMAL_PARAMETER_STEREOSCOPIC_MODE_T>()}");
            Console.WriteLine($"Hello World! Size of VC_IMAGE_T is {Marshal.SizeOf<VC_IMAGE_T>()}");
            Console.WriteLine($"Hello World! Size of MMAL_COMPONENT_T is {Marshal.SizeOf<MMAL_COMPONENT_T>()}");
            Console.WriteLine($"Hello World! Size of MMAL_PORT_T is {Marshal.SizeOf<MMAL_PORT_T>()}");
            Console.WriteLine($"Hello World! Size of MMAL_PARAMETER_CAMERA_INFO_CAMERA_T is {Marshal.SizeOf<MMAL_PARAMETER_CAMERA_INFO_CAMERA_T>()}");
            Console.WriteLine("ok");

            int camNumber = 0;
                CameraInfo camInfo;
            using (BcmHost host = new BcmHost())
            {
                Console.WriteLine("Host created");
                //set_sensor_defaults
                using (var camInfoComp = host.MMAL.ComponentCreateCameraInfo())
                {
                    Console.WriteLine("Getting camera info");
                    camInfo = camInfoComp.GetCameraInfo(camNumber);
                }
            }
            Console.WriteLine($"Camera info : name={camInfo.Name} res={camInfo.Width}x{camInfo.Height}");
        }
    }
}
