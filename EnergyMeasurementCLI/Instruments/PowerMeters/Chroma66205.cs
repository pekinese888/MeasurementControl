using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalInstruments.Visa;
using Ivi.Visa;

namespace EnergyMeasurementCLI.Instruments
{
    public sealed class Chroma66205 : PowerMeter
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

       public Dictionary<string, double> Measure(params MeasurementParameters[] measurementParameters)
        {
            throw new NotImplementedException();
        }

        public void ClearStatusByteRegister()
        {
            throw new NotImplementedException();
        }

        public new MessageBasedSession _session;

        public enum MeasurementParameters
        {
            V, //RMS Voltage
            VPKp, //Positive Voltage Peak 
            VPKn, //Negative Voltage Peak
            THDV, //Total Harmonic Distortion of Voltage
            I, //RMS Current
            IPKp, //Positive Current Peak
            IPKn, //Negative Current Peak
            IS,
            CFI, //Crest Factor Current
            THDI, //Total Harmonic Distortion of Current
            W, //Active Power
            PF, //Power Factor
            VA, //Apparent Power
            VAR, //Reactive Power
            WH, //Watt-hour
            FREQ, //Frequency
            VDC, //Voltage DC Value
            IDC, //Current DC Value
            WDC,//Wattage DC Value
            VMEAN, //Mean Voltage
            DEG, //Phase difference in degree
            CFV, //Crest Factor Voltage
            VHZ, //Voltage Frequency
            IHZ, //Current Frequency
            AH, //Ampere Hours
        }
    }
}
