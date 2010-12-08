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

        /// <summary>
        /// Gets the number of characters that the given user has on this realm.
        /// </summary>
        /// <param name="username">The user's username - likely stored in client.SRP.Username</param>
        /// <returns>However many characters this user has on this realm.</returns>
        public byte GetNumChars(string username)
        {
            // TODO implement
            return (byte)0;
        }
    }
}
