using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.DisplaySpyServer
{
    /// <summary>
    /// Image data
    /// </summary>
    public class ImageData
    {
        /// <summary>
        /// Datetime image has been taken
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Image content as jpeg
        /// </summary>
        public byte[] ImageJpeg { get; set; }

        internal ImageData Clone()
        {
            return (ImageData)this.MemberwiseClone();
        }
    }
}
