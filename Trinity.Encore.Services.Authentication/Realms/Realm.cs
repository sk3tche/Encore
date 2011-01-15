namespace Trinity.Encore.Services.Authentication.Realms
{
    // TODO add a way for the authserver to communicate with this particular realm
    /// <summary>
    /// Manages all data and communication that is specific to a single Realm - Auth-service connection.
    /// </summary>
    public sealed class Realm
    {
        #region Properties
        public byte Lock
        {
            get;
            private set;
        }

        public byte Icon
        {
            get;
            private set;
        }

        public byte Color
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public string Address
        {
            get;
            private set;
        }

        public float PopulationLevel
        {
            get;
            private set;
        }

        public byte TimeZone
        {
            get;
            private set;
        }
        #endregion

        public Realm(byte icon, byte locked, byte color, string name, string address, float populationLevel, byte timeZone)
        {
            Icon = icon;
            Lock = locked;
            Color = color;
            Name = name;
            Address = address;
            PopulationLevel = populationLevel;
            TimeZone = timeZone;
        }
    }
}
