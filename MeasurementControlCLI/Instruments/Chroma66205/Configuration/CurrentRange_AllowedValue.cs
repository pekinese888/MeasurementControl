namespace MeasurementControlCLI.Instruments.Chroma66205.Configuration
{
    public class CurrentRange_AllowedValue : Helper.AllowedValue
    {
        CurrentRange_AllowedValue(string _MessageBasedSessionRepresentation, string _StringRepresentation, string _Description) : base(_MessageBasedSessionRepresentation, _StringRepresentation, _Description){}

        /// <summary>
        /// AUTO Range
        /// </summary>
        public static readonly CurrentRange_AllowedValue AUTO = new CurrentRange_AllowedValue("AUTO", "AUTO", "AUTO Range");
        /// <summary>
        /// 30A Range
        /// </summary>
        public static readonly CurrentRange_AllowedValue V600 = new CurrentRange_AllowedValue("V600", "V600", "600V Range");
        /// <summary>
        /// 20A Range
        /// </summary>
        public static readonly CurrentRange_AllowedValue V300 = new CurrentRange_AllowedValue("V300", "V300", "300V Range");
        /// <summary>
        /// 5A Range
        /// </summary>
        public static readonly CurrentRange_AllowedValue V150 = new CurrentRange_AllowedValue("V150", "V150", "150V Range");
        /// <summary>
        /// 2A Range
        /// </summary>
        public static readonly CurrentRange_AllowedValue V60 = new CurrentRange_AllowedValue("V60", "V60", "60V Range");
        /// <summary>
        /// 0.5A Range
        /// </summary>
        public static readonly CurrentRange_AllowedValue V30 = new CurrentRange_AllowedValue("V30", "V30", "30V Range");
        /// <summary>
        /// 0.3A Range
        /// </summary>
        public static readonly CurrentRange_AllowedValue V15 = new CurrentRange_AllowedValue("V15", "V15", "15V Range");
        /// <summary>
        /// 0.2A Range
        /// </summary>
        public static readonly CurrentRange_AllowedValue V15 = new CurrentRange_AllowedValue("V15", "V15", "15V Range");
        /// <summary>
        /// 0.05A Range
        /// </summary>
        public static readonly CurrentRange_AllowedValue V15 = new CurrentRange_AllowedValue("V15", "V15", "15V Range");
        /// <summary>
        /// 0.02A Range
        /// </summary>
        public static readonly CurrentRange_AllowedValue V15 = new CurrentRange_AllowedValue("V15", "V15", "15V Range");
        /// <summary>
        /// 0.005A Range
        /// </summary>
        public static readonly CurrentRange_AllowedValue V15 = new CurrentRange_AllowedValue("V15", "V15", "15V Range");
    }
}
