using System;
using System.Diagnostics.Contracts;
using Trinity.Encore.Framework.Core.Threading.Actors;

namespace Trinity.Encore.Framework.Game.Threading
{
    public abstract class SingletonActor<T> : Actor
        where T : SingletonActor<T>
    {
        protected SingletonActor(Func<T> creator)
        {
            Contract.Requires(creator != null);

            _lazy = new Lazy<T>(creator);
        }

        private static Lazy<T> _lazy;

        public static T Instance
        {
            get { return _lazy.Value; }
        }
    }
}
