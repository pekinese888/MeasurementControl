namespace MeasurementControlCLI.Instruments.Chroma66205.InternalConfiguration
{
    public class Header_AllowedValue : Helper.AllowedValue
    {
        Header_AllowedValue(string _MessageBasedSessionRepresentation, string _StringRepresentation, string _Description) : base(_MessageBasedSessionRepresentation, _StringRepresentation, _Description){}

        /// <summary>
        /// Turns Return Headers On
        /// </summary>
        public static readonly Header_AllowedValue ON = new Header_AllowedValue("ON", "ON", "Turns Return Headers On");
        /// <summary>
        /// Turns Return Headers Off
        /// </summary>
        public static readonly Header_AllowedValue OFF = new Header_AllowedValue("OFF", "OFF", "Turns Return Headers On");
    }
}
