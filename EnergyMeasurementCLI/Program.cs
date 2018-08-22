using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ivi.Visa;

namespace EnergyMeasurementCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<string> resources = GlobalResourceManager.Find("");
            IEnumerator<string> res = resources.GetEnumerator();

        }
    }
}
