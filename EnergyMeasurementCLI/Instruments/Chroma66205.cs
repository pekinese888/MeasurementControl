using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalInstruments.Visa;
using Ivi.Visa;

namespace EnergyMeasurementCLI.Instruments
{
    public class Chroma66205 : PowerMeter
    {
        public Chroma66205(string resourceName) : base(resourceName)
        {
            try
            {
                _session = (MessageBasedSession)base._session;
                Console.WriteLine(_session.ResourceManufacturerName);
            }
            catch (Exception)
            {
                
            }
            
        }

        public Chroma66205(string resourceName, AccessModes accessModes, int timeoutMilliseconds) : base(resourceName, accessModes, timeoutMilliseconds)
        {
            _session = (MessageBasedSession)base._session;
        }

        /*~Chroma66205()
        {
            if (_session != null)
            {
                _session.Dispose();
            }
            
        }*/

       

        public void ClearStatusByteRegister()
        {
            throw new NotImplementedException();
        }

        public new MessageBasedSession _session;
    }
}
