using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Trinity.Core.Collections;
using Trinity.Encore.Game.Threading;

namespace Trinity.Encore.AuthenticationService.Realms
{
    public sealed class RealmManager : SingletonActor<RealmManager>
    {
        private readonly Dictionary<string, Realm> _realms = new Dictionary<string, Realm>();

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_realms != null);
        }

        public void AddRealm(Realm realm)
        {
            Contract.Requires(realm != null);

            _realms.Add(realm.Id, realm);
        }

        public void RemoveRealm(string id)
        {
            Contract.Requires(!string.IsNullOrEmpty(id));

            _realms.Remove(id);
        }

        public Realm GetRealm(string id)
        {
            return _realms.TryGet(id);
        }

        public Realm GetRealm(Func<Realm, bool> predicate)
        {
            Contract.Requires(predicate != null);

            return GetRealms(predicate).SingleOrDefault();
        }

        public IEnumerable<Realm> GetRealms(Func<Realm, bool> predicate)
        {
            Contract.Requires(predicate != null);

            return _realms.Values.Where(predicate).Force();
        }
    }
}
