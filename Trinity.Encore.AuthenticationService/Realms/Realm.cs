using System;
using System.Diagnostics.Contracts;
using Trinity.Encore.Game.Realms;

namespace Trinity.Encore.AuthenticationService.Realms
{
    public sealed class Realm
    {
        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(!string.IsNullOrEmpty(Id));
            Contract.Invariant(!string.IsNullOrEmpty(Name));
            Contract.Invariant(Location != null);
            Contract.Invariant(ClientVersion != null);
            Contract.Invariant(Capacity >= 0);
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public Uri Location { get; set; }

        public Version ClientVersion { get; set; }

        public RealmType Type { get; set; }

        public RealmStatus Status { get; set; }

        public RealmCategory Category { get; set; }

        public int Capacity { get; set; }

        public int Population { get; set; }

        public float PopulationLevel
        {
            get { return Population > Capacity * 0.75f ? 1.7f : Population > Capacity / 3.0f ? 1.6f : 1.5f; }
        }

        public RealmFlags Flags { get; set; }

        public Realm(string id, string name, Uri uri, Version version)
        {
            Contract.Requires(!string.IsNullOrEmpty(id));
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(uri != null);
            Contract.Requires(version != null);

            Id = id;
            Name = name;
            Location = uri;
            ClientVersion = version;
        }
    }
}
