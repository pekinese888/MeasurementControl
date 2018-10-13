using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasurementControlCLI.Instruments
{
    /// <summary>
    /// Generation Parameters for ALL Instruments
    /// </summary>
    public abstract class GenerationParameter
    {
        public new string ToString { get; protected set; }
        public string Description { get; protected set; }

        protected GenerationParameter(string toString, string description)
        {
            ToString = toString;
            Description = description;
        }
    }
}
