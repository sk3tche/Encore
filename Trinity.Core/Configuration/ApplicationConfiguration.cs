using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using Trinity.Core.Reflection;

namespace Trinity.Core.Configuration
{
    /// <summary>
    /// Defines a configuration made up of key/value pairs.
    /// </summary>
    public sealed class ApplicationConfiguration
    {
        private readonly System.Configuration.Configuration _cfg;

        private readonly List<ConfigurationInfo> _configs = new List<ConfigurationInfo>();

        /// <summary>
        /// Gets the executable file that this configuration is associated with.
        /// </summary>
        public FileInfo ExecutableFile { get; private set; }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(ExecutableFile != null);
            Contract.Invariant(_cfg != null);
            Contract.Invariant(_configs != null);
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="executablePath">The path of the executable whose AppConfig to load.</param>
        public ApplicationConfiguration(string executablePath)
        {
            Contract.Requires(!string.IsNullOrEmpty(executablePath));

            ExecutableFile = new FileInfo(executablePath);
            _cfg = ConfigurationManager.OpenExeConfiguration(ExecutableFile.FullName);
        }

        /// <summary>
        /// Opens the configuration file, reads any stored values, and inserts default values where needed.
        /// </summary>
        /// <returns>true if the file was opened; false if a new one was written.</returns>
        public bool Open()
        {
            var result = true;

            // Let's write the default values.
            if (!_cfg.HasFile || _cfg.AppSettings.Settings.Count == 0)
            {
                foreach (var cfg in _configs)
                {
                    var attr = cfg.Attribute;
                    var def = attr.Default;
                    var value = def == null ? string.Empty : def.ToString();
                    cfg.SetValue(def);
                    _cfg.AppSettings.Settings.Add(attr.Name, value);
                }

                Save();
                result = false;
            }

            Load();
            return result;
        }

        /// <summary>
        /// Loads configuration values to in-memory properties.
        /// </summary>
        public void Load()
        {
            foreach (var cfg in _configs)
            {
                var val = _cfg.AppSettings.Settings[cfg.Attribute.Name];
                Contract.Assume(val != null);
                Contract.Assume(val.Value != null);

                var value = ConvertType(val, cfg.Property.PropertyType);
                cfg.SetValue(value);
            }
        }

        /// <summary>
        /// Scans all loaded assemblies for configuration properties.
        /// </summary>
        public void ScanAll()
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                Contract.Assume(asm != null);
                ScanAssembly(asm);
            }
        }

        /// <summary>
        /// Scans a given assembly for configuration properties.
        /// </summary>
        /// <param name="asm">The assembly to scan.</param>
        public void ScanAssembly(Assembly asm)
        {
            Contract.Requires(asm != null);

            foreach (var type in asm.GetTypes())
            {
                foreach (var prop in type.GetProperties())
                {
                    Contract.Assume(prop != null);

                    var attr = prop.GetCustomAttribute<ConfigurationVariableAttribute>();

                    if (attr == null)
                        continue;

                    if (type.IsGenericType)
                        throw new ReflectionException("Config value is within a generic type.");

                    if (!prop.PropertyType.IsSimple())
                        throw new ReflectionException("Config value must be of a simple type.");

                    if (prop.GetIndexParameters().Length > 0)
                        throw new ReflectionException("Config values cannot be indexers.");

                    var accessors = prop.GetAccessors();

                    if (accessors.Length != 2)
                        throw new ReflectionException("Config value must have get/set accessors.");

                    foreach (var accessor in accessors)
                    {
                        if (!accessor.IsStatic)
                            throw new ReflectionException("Config value must not have static accessor.");

                        if (!accessor.IsPublic)
                            throw new ReflectionException("Config value must not have non-public accessor.");
                    }

                    _configs.Add(new ConfigurationInfo(prop, attr));
                }
            }
        }

        private static object ConvertType(KeyValueConfigurationElement val, Type type)
        {
            Contract.Requires(val != null);
            Contract.Requires(val.Value != null);
            Contract.Requires(type != null);

            try
            {
                return val.Value.Cast(type);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Saves all in-memory configuration values.
        /// </summary>
        public void Save()
        {
            foreach (var cfg in _configs.Where(x => !x.Attribute.Static))
                _cfg.AppSettings.Settings[cfg.Attribute.Name].Value = cfg.GetValue().ToString();

            _cfg.Save(ConfigurationSaveMode.Full);
        }
    }
}
