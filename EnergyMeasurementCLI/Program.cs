using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalInstruments.Visa;

namespace EnergyMeasurementCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            //Example for Message Based Communication
            ResourceManager resourceManager = new ResourceManager();
            IEnumerable<string> TcpipInstruments = resourceManager.Find("TCPIP?*INSTR");
            TcpipSession session = new TcpipSession(TcpipInstruments.First());
            session.FormattedIO.WriteLine("FETC?V,I,W");
            Console.WriteLine(session.FormattedIO.ReadString());
            session.Dispose();
            resourceManager.Dispose();
        }
    }
}
