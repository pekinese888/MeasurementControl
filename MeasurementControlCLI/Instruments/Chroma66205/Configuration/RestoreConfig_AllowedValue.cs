using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasurementControlCLI.Instruments.Chroma66205.Configuration
{
    public class RestoreConfig_AllowedValue : Helper.AllowedValue
    {
        RestoreConfig_AllowedValue(string _MessageBasedSessionRepresentation, string _StringRepresentation, string _Description) : base(_MessageBasedSessionRepresentation, _StringRepresentation, _Description){}


        /// <summary>
        /// FactoryDefaults
        /// </summary>
        public static readonly RestoreConfig_AllowedValue FactoryDefaults = new RestoreConfig_AllowedValue("0", "FactoryDefaults", "FactoryDefaults");
        /// <summary>
        /// UserConfig1
        /// </summary>
        public static readonly RestoreConfig_AllowedValue UserConfig1 = new RestoreConfig_AllowedValue("1", "UserConfig1", "UserConfig1");
        /// <summary>
        /// UserConfig2
        /// </summary>
        public static readonly RestoreConfig_AllowedValue UserConfig2 = new RestoreConfig_AllowedValue("2", "UserConfig2", "UserConfig2");
        /// <summary>
        /// UserConfig3
        /// </summary>
        public static readonly RestoreConfig_AllowedValue UserConfig3 = new RestoreConfig_AllowedValue("3", "UserConfig3", "UserConfig3");
        /// <summary>
        /// UserConfig4
        /// </summary>
        public static readonly RestoreConfig_AllowedValue UserConfig4 = new RestoreConfig_AllowedValue("4", "UserConfig4", "UserConfig4");
        /// <summary>
        /// UserConfig5
        /// </summary>
        public static readonly RestoreConfig_AllowedValue UserConfig5 = new RestoreConfig_AllowedValue("5", "UserConfig5", "UserConfig5");
        /// <summary>
        /// UserConfig6
        /// </summary>
        public static readonly RestoreConfig_AllowedValue UserConfig6 = new RestoreConfig_AllowedValue("6", "UserConfig6", "UserConfig6");
        /// <summary>
        /// UserConfig7
        /// </summary>
        public static readonly RestoreConfig_AllowedValue UserConfig7 = new RestoreConfig_AllowedValue("7", "UserConfig7", "UserConfig7");
        /// <summary>
        /// UserConfig8
        /// </summary>
        public static readonly RestoreConfig_AllowedValue UserConfig8 = new RestoreConfig_AllowedValue("8", "UserConfig8", "UserConfig8");
        /// <summary>
        /// UserConfig9
        /// </summary>
        public static readonly RestoreConfig_AllowedValue UserConfig9 = new RestoreConfig_AllowedValue("9", "UserConfig9", "UserConfig9");
    }
}
