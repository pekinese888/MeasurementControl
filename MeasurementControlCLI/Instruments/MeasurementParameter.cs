using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasurementControlCLI.Instruments
{
    /// <summary>
    /// Measurement Parameters common to ALL Instruments
    /// </summary>
    public class MeasurementParameter
    {
        public new string ToString { get; protected set; }
        public string Description { get; protected set; }

        protected MeasurementParameter(string toString, string description)
        {
            ToString = toString;
            Description = description;
        }
    }
}
