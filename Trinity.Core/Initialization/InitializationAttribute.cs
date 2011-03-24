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

        /// <summary>
        /// The name of the initializable routine.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The pass in which the routine is to be executed.
        /// </summary>
        /// <remarks>
        /// Note that the routine may not strictly be executed in the given pass, depending on the dependency
        /// hierarchy. This property is just a hint.
        /// </remarks>
        public InitializationPass Pass { get; private set; }

        /// <summary>
        /// A type that must be initialized before this routine is initialized.
        /// </summary>
        public Type Dependency { get; set; }
    }
}
