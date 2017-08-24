using ImageSharp;
using ImageSharp.Formats;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SixLabors.Fonts;
using SixLabors.Fonts.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace RaspberryPi.DisplaySpyServer
{
    internal sealed class ImageGrabberService : IHostedService
    {
        private readonly IScreenCapture m_imageProvider;
        private readonly DisplaySpyServerOption m_options;
        private readonly Thread m_th;
        private CancellationTokenSource m_cts;
        private readonly ILogger<ImageGrabberService> m_logger;
        private BufferBlock<ImageData> m_inputBuffer;
        private TransformBlock<ImageData, ImageData> m_transform;
        private BroadcastBlock<ImageData> m_output;
        private ActionBlock<ImageData> m_imageLogger;
        private FontFamily m_arial;

        public ImageGrabberService(IScreenCapture imageProvider, IOptions<DisplaySpyServerOption> options, ILogger<ImageGrabberService> logger)
        {
            this.m_imageProvider = imageProvider;
            this.m_options = options?.Value ?? new DisplaySpyServerOption();
            this.m_th = new Thread(this.Run);
            this.m_th.Name = this.GetType().Name;
            this.m_logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.m_logger.LogInformation("ImageGrabberService.StartAsync");
            this.m_cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            this.m_cts.Token.Register(() =>
            {
                this.m_logger.LogInformation("ImageGrabberService cancellation requested");
            });

            this.m_inputBuffer = new BufferBlock<ImageData>(new DataflowBlockOptions { CancellationToken = this.m_cts.Token });
            this.m_transform = new TransformBlock<ImageData, ImageData>(new Func<ImageData, ImageData>(this.TransformImage), new ExecutionDataflowBlockOptions { CancellationToken = this.m_cts.Token });
            this.m_output = new BroadcastBlock<ImageData>(img => img.Clone());
            this.m_imageLogger = new ActionBlock<ImageData>(img =>
            {
                this.m_logger.LogInformation("Got image");
            });

            this.m_inputBuffer.LinkTo(this.m_transform);
            this.m_inputBuffer.Completion.ContinueWith(t =>
            {
                this.m_logger.LogInformation("input buffer completed");
                if (t.IsFaulted) ((IDataflowBlock)this.m_transform).Fault(t.Exception);
                else
                    this.m_transform.Complete();
            });
            this.m_transform.LinkTo(this.m_output);
            this.m_transform.Completion.ContinueWith(t =>
            {
                this.m_logger.LogInformation("transform block completed");
                if (t.IsFaulted) ((IDataflowBlock)this.m_output).Fault(t.Exception);
                else
                    this.m_output.Complete();
            });
            this.m_output.LinkTo(this.m_imageLogger);
            this.m_output.Completion.ContinueWith(t =>
            {
                this.m_logger.LogInformation("output block completed");
                if (t.IsFaulted) ((IDataflowBlock)this.m_imageLogger).Fault(t.Exception);
                else
                    this.m_imageLogger.Complete();
            });

            try
            {
                this.m_arial = SystemFonts.Find("arial");
            }
            catch (FontFamilyNotFountException)
            {
                int count = SystemFonts.Collection.Families.Count();
                this.m_logger.LogError($"Arial not found. {count} fonts available.");
                this.m_arial = SystemFonts.Collection.Families.FirstOrDefault();
                if (count > 0)
                {
                    foreach (var ff in SystemFonts.Collection.Families)
                    {
                        this.m_logger.LogInformation($"Font : {ff.Name}");
                    }
                }
            }

            this.m_th.Start(this.m_cts.Token);
            return Task.CompletedTask;
        }

        internal IDisposable LinkOutputTo(ITargetBlock<ImageData> target)
        {
            return this.m_output.LinkTo(target);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.m_logger.LogInformation("ImageGrabberService.StopAsync");
            this.m_cts.Cancel();
            this.m_th.Join();
            return Task.CompletedTask;
        }

        private void Run(object state)
        {
            CancellationToken token = (CancellationToken)state;
            Stopwatch sw = Stopwatch.StartNew();
            TimeSpan waitDelay = TimeSpan.FromSeconds(1d / this.m_options.FPS);
            Console.WriteLine("waitDelay " + waitDelay);
            try
            {
                while (!token.IsCancellationRequested)
                {
                    TimeSpan start = sw.Elapsed;
                    ImageData data = this.m_imageProvider.CaptureImage();
                    this.m_inputBuffer.Post(data);
                    TimeSpan duration = sw.Elapsed - start;
                    TimeSpan wait = waitDelay - duration;
                    if (wait > TimeSpan.Zero)
                    {
                        Thread.Sleep(wait);
                        Thread.Yield();
                    }
                    else
                    {
                        Thread.Sleep(1);
                        Thread.Yield();
                    }
                }
            }
            finally
            {
                sw.Stop();
                this.m_inputBuffer.Complete();
            }
        }

        private ImageData TransformImage(ImageData data)
        {
            using (Image<Rgba32> img = Image.Load(data.ImageJpeg))
            using (MemoryStream output = new MemoryStream())
            {
                img
                    .DrawText(data.Date.ToLongTimeString(), new Font(this.m_arial, 50, FontStyle.Regular), Rgba32.Red, new SixLabors.Primitives.PointF(0, 0))
                    .SaveAsJpeg(output, new JpegEncoder
                    {
                        Quality = 50,
                    });
                data.ImageJpeg = output.ToArray();
            }
            return data;
        }

        //protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        //{
        //    int fps = 1;
        //    Stopwatch sw = Stopwatch.StartNew();
        //    TimeSpan waitDelay = TimeSpan.FromSeconds(1d / fps);
        //    Console.WriteLine("waitDelay " + waitDelay);
        //    try
        //    {
        //        while (!cancellationToken.IsCancellationRequested)
        //        {
        //            TimeSpan start = sw.Elapsed;
        //            this.m_imageProvider.ProduceImage();
        //            TimeSpan duration = sw.Elapsed - start;
        //            TimeSpan wait = waitDelay - duration;
        //            Console.WriteLine($"duration {duration} waitDelay {waitDelay} wait {wait}");
        //            if (wait > TimeSpan.Zero)
        //            {
        //                await Task.Delay(wait);
        //            }
        //            else
        //            {
        //                await Task.Delay(1);
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        sw.Stop();
        //        this.m_imageProvider.Complete();
        //    }
        //}
    }
}
