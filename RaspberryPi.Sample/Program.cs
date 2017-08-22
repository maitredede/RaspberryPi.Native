using Microsoft.Extensions.PlatformAbstractions;
using System;

namespace RaspberryPi.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var runtimeInfo = PlatformServices.Default.Application.RuntimeFramework;
            Console.WriteLine("Platform full name is {0}", runtimeInfo.FullName);
            Console.WriteLine("Identifier is {0}", runtimeInfo.Identifier);
            Console.WriteLine("Profile is {0}", runtimeInfo.Profile);
            Console.WriteLine("Version is {0}", runtimeInfo.Version);

            Console.WriteLine("GPU temp is " + MetricsHelper.GpuTemp());
            Console.WriteLine("CPU temp is " + MetricsHelper.CpuTemp());
            Console.WriteLine("Memory CPU/GPU is " + MetricsHelper.DedicatedMemoryCpu() + "/" + MetricsHelper.DedicatedMemoryGpu());
        }
    }
}
