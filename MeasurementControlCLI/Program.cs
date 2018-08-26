using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalInstruments.Visa;
using Ivi.Visa;
using MeasurementControlCLI.Instruments.PowerMeters;

namespace MeasurementControlCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            //Example for Message Based Communication with TCPIP0::192.168.1.7::inst0::INSTR
            /*
            ResourceManager resourceManager = new ResourceManager();
            IEnumerable<string> TcpipInstruments = resourceManager.Find("?*INSTR");
            foreach (string str in TcpipInstruments)
            {
                Console.WriteLine(str);
            }
            TcpipSession session = new TcpipSession(TcpipInstruments.First());
            session.FormattedIO.WriteLine("FETC?V,I,W");
            Console.WriteLine(session.FormattedIO.ReadString());
            session.Dispose();
            resourceManager.Dispose();
            

            using (ResourceManager rsm = new ResourceManager())
            {
                foreach (string str in rsm.Find("?*INSTR"))
                {
                    Console.WriteLine(str);
                }
            }
            */

            using (Instruments.Instrument chroma66205 = new Chroma66205("TCPIP0::192.168.1.7::inst0::GEN"))
            {
                chroma66205.Measure(
                    Chroma66205.MeasurementParameter.W,
                    Chroma66205.MeasurementParameter.V,
                    Chroma66205.MeasurementParameter.I,
                    Chroma66205.MeasurementParameter.THDV);
            }

        }
    }
}
