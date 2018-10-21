namespace MeasurementControlCLI.Instruments.Chroma66205.InternalConfiguration
{
    public class TransmitTerminator_AllowedValue : Helper.AllowedValue
    {
        TransmitTerminator_AllowedValue(string _MessageBasedSessionRepresentation, string _StringRepresentation, string _Description) : base(_MessageBasedSessionRepresentation, _StringRepresentation, _Description){}

        /// <summary>
        /// Set Transmit Terminator to LineFeed
        /// </summary>
        public static readonly TransmitTerminator_AllowedValue LineFeed = new TransmitTerminator_AllowedValue("0", "\n", "LineFeed");
        /// <summary>
        /// Set Transmit Terminator to LineFeed and CarriageReturn
        /// </summary>
        public static readonly TransmitTerminator_AllowedValue LineFeedAndCarriageReturn = new TransmitTerminator_AllowedValue("1", "\n\r", "LineFeed and CarriageReturn");
    }
}
