using System;
using JetBrains.Annotations;

namespace Trinity.Core.Initialization
{
    /// <summary>
    /// Used to mark a method as initializable (i.e. it is called on startup and shutdown).
    /// </summary>
    [MeansImplicitUse]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class InitializableAttribute : Attribute
    {
        public InitializableAttribute(string name, InitializationPass pass)
        {
            Name = name;
            Pass = pass;
        }

        public string Name { get; private set; }

        public InitializationPass Pass { get; private set; }

        public Type Dependency { get; set; }
    }
}
