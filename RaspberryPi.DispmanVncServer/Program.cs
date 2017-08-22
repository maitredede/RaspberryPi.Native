using System;
using RaspberryPi.LibVncServer;
using System.Runtime.InteropServices;
using System.Reflection;
using static RaspberryPi.Utils;

namespace RaspberryPi.DispmanVncServer
{
    public static class Program
    {
        const short BPP = 2;
        public static void Main(string[] args)
        {
            try
            {
                using (BcmHost host = new BcmHost())
                using (Display display = host.Dispman.DisplayOpen(Screen.MAIN_LCD))
                using (DispmanVncServerDisplay src = new DispmanVncServerDisplay(host, display))
                {
                    src.Run();
                }
                Console.WriteLine("ok");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                do
                {
                    Console.WriteLine(ex.GetType().FullName + ": " + ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
                while ((ex = ex.InnerException) != null);
                Console.ReadLine();
            }
        }

    }
}
