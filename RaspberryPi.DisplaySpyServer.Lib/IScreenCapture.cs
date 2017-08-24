using ImageSharp;
using ImageSharp.Formats;
using SixLabors.Fonts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Threading;

namespace RaspberryPi.DisplaySpyServer
{
    internal interface IScreenCapture
    {
        ImageData CaptureImage();
    }
}
