using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using Trinity.Core.Logging;
using Trinity.Core.Reflection;

namespace Trinity.Core.Initialization
{
    /// <summary>
    /// Handles initialization of types.
    /// </summary>
    public static class InitializationManager
    {
        private static readonly Dictionary<InitializationPass, List<InitializationInfo>> _initializers =
            new Dictionary<InitializationPass, List<InitializationInfo>>();

        private static readonly LogProxy _log = new LogProxy("InitializationManager");

        [SuppressMessage("Microsoft.Performance", "CA1810", Justification = "Initialization must be done in a static constructor.")]
        static InitializationManager()
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                Contract.Assume(asm != null);
                Initialize(asm);
            }
        }

        /// <summary>
        /// Initializes all types within an assembly.
        /// </summary>
        /// <param name="asm">The assembly to initialize.</param>
        public static void Initialize(Assembly asm)
        {
            Contract.Requires(asm != null);

            // First, grab all initializable methods...
            foreach (var type in asm.GetTypes())
            {
                foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
                {
                    Contract.Assume(method != null);

                    var attr = method.GetCustomAttribute<InitializableAttribute>();

                    if (attr == null)
                        continue;

                    if (type.IsGenericType)
                        throw new ReflectionException("Initialization method is within a generic type.");

                    if (!method.IsPublic)
                        throw new ReflectionException("Initialization method must be public.");

                    if (method.IsGenericMethod)
                        throw new ReflectionException("Initialization method must not be generic.");

                    if (method.ReturnType != typeof(void))
                        throw new ReflectionException("Invalid initialization method return type.");

                    var param = method.GetParameters();

                    if (param.Length != 1)
                        throw new ReflectionException("Invalid initialization method parameter count.");

                    if (param[0].ParameterType != typeof(bool))
                        throw new ReflectionException("Invalid initialization method parameter type.");

                    var init = new InitializationInfo(attr, method);

                    var pass = attr.Pass;
                    if (!_initializers.ContainsKey(pass))
                        _initializers[pass] = new List<InitializationInfo>();

                    _initializers[pass].Add(init);
                }
            }
        }

        private static void RunInitializable(IEnumerable<InitializationInfo> list, InitializationInfo init, bool value)
        {
            Contract.Requires(list != null);
            Contract.Requires(init != null);

            // Prevent duplicate initialization/teardown.
            if (init.Initialized == value)
                return;

            var attr = init.Attribute;
            var type = init.Method.DeclaringType;
            var depType = attr.Dependency;

            if (value && depType != null)
            {
                var dep = list.FirstOrDefault(x => x.Method.DeclaringType == depType);

                if (dep != null)
                    if (!dep.Initialized)
                        RunInitializable(list, dep, true);
            }

            if (!value)
            {
                var dep = list.FirstOrDefault(x => x.Attribute.Dependency == type);

                if (dep != null)
                    if (dep.Initialized)
                        RunInitializable(list, dep, false);
            }

            var initType = value ? "initialization" : "teardown";

            try
            {
                init.Method.Invoke(null, new object[] { value });
                init.Initialized = value;

                _log.Info("Completed {0} of {1} successfully.", initType, type.Name);
            }
            catch (TargetInvocationException e)
            {
                var ex = e.InnerException;

                _log.Error("Error in {0} of {1}:", initType, init.Attribute.Name);
                _log.Error("{0}", ex.Message);
                _log.Error("{0}", ex.StackTrace);
            }
        }

        /// <summary>
        /// Initializes all types in all assemblies within the current application domain.
        /// </summary>
        public static void InitializeAll()
        {
            var max = (InitializationPass)ReflectionUtility.GetEnumValueCount<InitializationPass>();

            for (InitializationPass i = 0; i < max; i++)
            {
                List<InitializationInfo> list;
                _initializers.TryGetValue(i, out list);

                if (list == null)
                    continue;

                foreach (var init in list)
                {
                    Contract.Assume(init != null);
                    RunInitializable(list, init, true);
                }
            }
        }

        /// <summary>
        /// Performs reverse-initialization (teardown) on all initializers.
        /// </summary>
        public static void TeardownAll()
        {
            var max = (int)((InitializationPass)ReflectionUtility.GetEnumValueCount<InitializationPass>() - 1);

            for (var i = max; i >= 0; i--)
            {
                List<InitializationInfo> list;
                _initializers.TryGetValue((InitializationPass)i, out list);

                if (list == null)
                    continue;

                foreach (var init in list)
                {
                    Contract.Assume(init != null);
                    RunInitializable(list, init, false);
                }
            }
        }
    }
}
