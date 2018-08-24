using NationalInstruments.Visa;
using Ivi.Visa;
using System;

namespace EnergyMeasurementCLI.Instruments
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