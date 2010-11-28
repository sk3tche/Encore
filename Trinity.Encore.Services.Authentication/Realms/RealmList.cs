using System.Diagnostics.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Trinity.Encore.Services.Authentication.Realms
{
    public static class RealmList
    {
        private static readonly Dictionary<string, Realm> realms;

        static RealmList()
        {
            realms = new Dictionary<string, Realm>();
        }

        public static void UpdateRealm(Realm realm)
        {
            Contract.Requires(realm != null);
            if (realms.Keys.Contains(realm.Name))
                realms[realm.Name] = realm;
            else
                realms.Add(realm.Name, realm);
        }

        public static Realm GetRealm(string realmName)
        {
            Contract.Requires(realmName != null);
            return realms[realmName];
        }

        public static void RemoveRealm(string realmName)
        {
            Contract.Requires(realmName != null);
            realms.Remove(realmName);
        }

        public static List<string> GetRealmNames()
        {
            return realms.Keys.ToList();
        }
    }
}
