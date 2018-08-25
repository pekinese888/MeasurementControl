using NationalInstruments.Visa;
using Ivi.Visa;
using System;

namespace MeasurementControlCLI.Instruments
{
    public abstract class PowerMeter : Instrument
    {
        public PowerMeter(string resourceName):base(resourceName)
        {
           
        }

        public PowerMeter(string resourceName, AccessModes accessModes, int timeoutMilliseconds):base(resourceName, accessModes, timeoutMilliseconds)
        {
           
        }     
    }
}