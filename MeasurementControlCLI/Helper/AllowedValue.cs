namespace MeasurementControlCLI.Helper
{
    public  abstract class AllowedValue
    {
        public string MessageBasedSessionRepresentation { get; protected set; }
        public string StringRepresentation { get; protected set; }
        public string Description { get; protected set; }

        protected AllowedValue(string _MessageBasedSessionRepresentation, string _StringRepresentation, string _Description)
        {
            MessageBasedSessionRepresentation = _MessageBasedSessionRepresentation;
            StringRepresentation = _StringRepresentation;
            Description = _Description;
        }
    }
}
