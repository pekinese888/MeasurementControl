namespace MeasurementControlCLI.Instruments.Chroma66205.InternalConfiguration
{
    public class TransmitSeperator_AllowedValue : Helper.AllowedValue
    {
        TransmitSeperator_AllowedValue(string _MessageBasedSessionRepresentation, string _StringRepresentation, string _Description) : base(_MessageBasedSessionRepresentation, _StringRepresentation, _Description){}

        /// <summary>
        /// Set Transmit Seperator to Comma
        /// </summary>
        public static readonly TransmitSeperator_AllowedValue Comma = new TransmitSeperator_AllowedValue("0", ",", "Set Transmit Seperator to Comma");
        /// <summary>
        /// Set Transmit Seperator to Semicolon
        /// </summary>
        public static readonly TransmitSeperator_AllowedValue Semicolon = new TransmitSeperator_AllowedValue("1", ";", "Set Transmit Seperator to Semicolon");
    }
}
