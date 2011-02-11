using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Trinity.Core.Collections;
using Trinity.Encore.Game.Threading;

namespace Trinity.Encore.AuthenticationService.Sessions
{
    public sealed class SessionManager : SingletonActor<SessionManager>
    {
        private readonly Dictionary<string, SessionInfo> _sessions = new Dictionary<string, SessionInfo>();

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_sessions != null);
        }

        public void AddSession(SessionInfo session)
        {
            Contract.Requires(session != null);

            _sessions.Add(session.SRP.UserName, session);
        }

        public SessionInfo GetSession(string userName)
        {
            Contract.Requires(!string.IsNullOrEmpty(userName));

            return _sessions.TryGet(userName);
        }

        public void RemoveSession(string userName)
        {
            Contract.Requires(!string.IsNullOrEmpty(userName));

            _sessions.Remove(userName);
        }

        public bool IsActive(string userName)
        {
            Contract.Requires(!string.IsNullOrEmpty(userName));

            return _sessions.ContainsKey(userName);
        }

        public void SetActive(string userName, bool active)
        {
            Contract.Requires(!string.IsNullOrEmpty(userName));

            var session = _sessions.TryGet(userName);
            if (session != null)
                session.Active = active;
        }
    }
}
