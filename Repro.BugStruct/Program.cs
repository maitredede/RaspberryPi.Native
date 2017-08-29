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
        public const int MMAL_PARAMETER_CAMERA_INFO_MAX_STR_LEN = 16;

        static void Main(string[] args)
        {
            Console.WriteLine($"Size direct : MyClass {Marshal.SizeOf<MyClass>()}");
            Console.WriteLine($"Size direct : MyStruct {Marshal.SizeOf<MyStruct>()}");
            Test<MyClass>();
            Test<MyStruct>();

            new Generic<MyClass>().PrintSize();
            new Generic<MyStruct>().PrintSize();

            var g1 = new GenericCtor<MyClass>();
            var g2 = new GenericCtor<MyStruct>();

            var g3 = new HeritedGenericStruct();
            Console.WriteLine($"Size HeritedGenericStruct:  {g3.Size}");
            var g4 = new HeritedGenericClass();
            Console.WriteLine($"Size HeritedGenericClass:  {g4.Size}");


            Console.WriteLine("ok");

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

        private static void Test<T>()
        {
            Console.WriteLine($"Size generic method : {typeof(T).Name} {Marshal.SizeOf<T>()}");

        }
    }
}
