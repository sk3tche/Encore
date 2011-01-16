using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Trinity.Encore.AuthenticationService.Realms
{
    /// <summary>
    /// Serves to store and manage operations on all realms that communicate with this given auth service.
    /// </summary>
    public static class RealmList
    {
        private static readonly Dictionary<string, Realm> _realms = new Dictionary<string, Realm>();

        /// <summary>
        /// Updates the given realm's data if it exists, otherwise add the realm.
        /// </summary>
        /// <param name="realm">The realm to add to update. Must not be null.</param>
        public static void UpdateRealm(Realm realm)
        {
            Contract.Requires(realm != null);
            if (_realms.Keys.Contains(realm.Name))
                _realms[realm.Name] = realm;
            else
                _realms.Add(realm.Name, realm);
        }

        /// <summary>
        /// Gets the Realm instance corresponding to this particular name.
        /// </summary>
        /// <param name="realmName">The name of the realm to search for. Must not be null.</param>
        /// <returns>The Realm instance for this realm.</returns>
        public static Realm GetRealm(string realmName)
        {
            Contract.Requires(realmName != null);
            return _realms[realmName];
        }

        /// <summary>
        /// Removes the given realm from the list.
        /// </summary>
        /// <param name="realmName">The name of the realm to remove. Must not be null.</param>
        public static void RemoveRealm(string realmName)
        {
            Contract.Requires(realmName != null);
            _realms.Remove(realmName);
        }

        /// <summary>
        /// Gets a list of realm names.
        /// </summary>
        /// <value>A list of realm names that correspond to extant realms in this list.</value>
        public static IEnumerable<string> RealmNames
        {
            get { return _realms.Keys.ToList(); }
        }
    }
}
