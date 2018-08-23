using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalInstruments.Visa;
using Ivi.Visa;

namespace EnergyMeasurementCLI
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
            */

            ;
            Chroma66205 instrument = new Chroma66205("TCPIP0::192.168.1.7::inst0::INSTR");
        }
    }
}
