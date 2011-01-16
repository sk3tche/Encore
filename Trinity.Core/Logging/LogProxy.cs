using System.Diagnostics.Contracts;
using JetBrains.Annotations;

namespace Trinity.Core.Logging
{
    /// <summary>
    /// A "proxy" to be instantiated in classes for shorter logging calls.
    /// </summary>
    public sealed class LogProxy
    {
        public string Source { get; private set; }

        public LogProxy(string classSource)
        {
            Contract.Requires(!string.IsNullOrEmpty(classSource));

            Source = classSource;
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(!string.IsNullOrEmpty(Source));
        }

        [StringFormatMethod("format")]
        public void Info(string format, params object[] args)
        {
            Contract.Requires(format != null);
            Contract.Requires(args != null);

            LogManager.Information(Source, format, args);
        }

        [StringFormatMethod("format")]
        public void Warn(string format, params object[] args)
        {
            Contract.Requires(format != null);
            Contract.Requires(args != null);

            LogManager.Warning(Source, format, args);
        }

        [StringFormatMethod("format")]
        public void Error(string format, params object[] args)
        {
            Contract.Requires(format != null);
            Contract.Requires(args != null);

            LogManager.Error(Source, format, args);
        }
    }
}
