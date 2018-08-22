using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalInstruments.VisaNS;

namespace EnergyMeasurementCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            ResourceManager resourceManager = ResourceManager.GetLocalManager();
            string[] resources = resourceManager.FindResources("TCPIP?*INSTR");
            foreach (string str in resources)
            {
                Console.WriteLine(str);
            }
            TcpipSession session = new TcpipSession(resources[0]);
            Console.WriteLine(session.Query("FETC?V,I,W\n"));
            session.Dispose();
        }
    }
}
