using System;
using System.Reflection;
using Trinity.Core;
using Trinity.Core.Reflection;
using Trinity.Core.Threading.Actors;

namespace Trinity.Encore.Game.Threading
{
    public abstract class SingletonActor<T> : Actor<T>
        where T : SingletonActor<T>
    {
        private static readonly Lazy<T> _lazy = new Lazy<T>(() =>
        {
            var type = typeof(T);

            if (!type.IsSealed)
                throw new ReflectionException("Type {0} cannot be a singleton, as it is inheritable.".Interpolate(type));

            var ctors = type.GetConstructors();

            if (ctors.Length > 0)
                throw new ReflectionException("Type {0} cannot be a singleton, as it has public constructors.".Interpolate(type));

            var ctor = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);

            if (ctor == null || !ctor.IsPrivate)
                throw new ReflectionException("Type {0} cannot be a singleton, as it has no private constructor.".Interpolate(type));

            return (T)ctor.Invoke(null);
        });

        public static T Instance
        {
            get { return _lazy.Value; }
        }
    }
}
