using System;
using JetBrains.Annotations;

namespace Trinity.Core.Configuration
{
    /// <summary>
    /// Indicates that a property is a configuration value, and can be parsed by
    /// the AppConfig class.
    /// </summary>
    [MeansImplicitUse]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class ConfigurationVariableAttribute : Attribute
    {
        public ConfigurationVariableAttribute(string name, object def)
        {
            Name = name;
            Default = def;
        }

        /// <summary>
        /// The name of the value in the configuration file.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The default value. The string equivalent of this object will be written.
        /// </summary>
        public object Default { get; private set; }

        /// <summary>
        /// If true, this value won't be persisted to the configuration file on save.
        /// </summary>
        public bool Static { get; set; }
    }
}
