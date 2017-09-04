using System;
using System.Threading;
using System.Threading.Tasks;

namespace RaspberryPi.Native.LibNFC.NfcPoll
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Using libnfc {0}", NfcContext.Version);

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) =>
            {
                if (cts.IsCancellationRequested)
                {
                    //Hard break, let it go
                }
                else
                {
                    //soft break, handle it
                    e.Cancel = true;
                    cts.Cancel();
                }
            };

            byte pollNr = 20;
            byte period = 2;
            var modulations = new[]
            {
                new NfcModulation(NfcModulationType.ISO14443A, NfcBaudRate.BR106),
                new NfcModulation(NfcModulationType.ISO14443B, NfcBaudRate.BR106),
                new NfcModulation(NfcModulationType.Felica, NfcBaudRate.BR212),
                new NfcModulation(NfcModulationType.Felica, NfcBaudRate.BR424),
                new NfcModulation(NfcModulationType.Jewel, NfcBaudRate.BR106),
            };
            using (NfcContext context = new NfcContext())
            using (NfcDevice device = context.Open(null))
            {
                NfcInitiator initiator = device.InitInitiator(false);
                Console.WriteLine("NFC reader: {0} opened", device.Name);
                Console.WriteLine($"NFC device will poll during {pollNr * modulations.Length * period * 150}ms ({pollNr} pollings of {period * 150}ms for {modulations.Length} modulations)");

                //NfcInitiatorTarget target = await initiator.PollAsync(modulations, pollNr, period);
            }
        }
    }
}
