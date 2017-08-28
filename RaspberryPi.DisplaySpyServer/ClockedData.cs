using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaspberryPi.DisplaySpyServer
{
    public class ClockedData
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("cpuTemp")]
        public double CpuTemp { get; set; }
        [JsonProperty("gpuTemp")]
        public double GpuTemp { get; set; }
    }
}
