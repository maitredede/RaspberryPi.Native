using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace RaspberryPi.Camera
{
    public sealed class RaspiStillImageProvider : ICameraStillImageProvider
    {
        public Task<byte[]> CaptureImage(CaptureImageConfig parameters)
        {
            TaskCompletionSource<byte[]> tcs = new TaskCompletionSource<byte[]>();

            Tuple<CaptureImageConfig, TaskCompletionSource<byte[]>> state = new Tuple<CaptureImageConfig, TaskCompletionSource<byte[]>>(parameters, tcs);
            ThreadPool.QueueUserWorkItem(CaptureImage, state);
            return tcs.Task;
        }

        private static void CaptureImage(object state)
        {
            Tuple<CaptureImageConfig, TaskCompletionSource<byte[]>> args = (Tuple<CaptureImageConfig, TaskCompletionSource<byte[]>>)state;
            var tcs = args.Item2;
            try
            {
                ProcessStartInfo procStart = new ProcessStartInfo("raspistill");
                procStart.RedirectStandardError = true;
                procStart.RedirectStandardInput = true;
                procStart.RedirectStandardOutput = true;
                procStart.Arguments = args.Item2.ToString();

                string filename = Path.GetTempFileName();

                Process proc = new Process();
                proc.StartInfo = procStart;
                proc.Exited += (s, e) =>
                {
                    try
                    {
                        if (proc.ExitCode != 0)
                        {
                            tcs.SetException(new InvalidProgramException($"Error occured, process return with code {proc.ExitCode}"));
                        }
                        else
                        {
                            using (FileStream fs = File.OpenRead(filename))
                            using (MemoryStream ms = new MemoryStream())
                            {
                                fs.CopyTo(ms);
                                tcs.TrySetResult(ms.ToArray());
                            }
                        }
                    }
                    finally
                    {
                        proc.Dispose();
                        try
                        {
                            File.Delete(filename);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine("Can't delete temp file " + filename);
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
        }
    }
}
