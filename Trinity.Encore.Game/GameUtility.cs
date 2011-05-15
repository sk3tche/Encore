using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Trinity.Encore.Game
{
    public static class GameUtility
    {
        private static readonly Dictionary<string, ClientType> _clientTypeMapping = new Dictionary<string, ClientType>
        {
            { "WoWT", ClientType.Test },
            { "WoWB", ClientType.Beta },
            { "WoW\0", ClientType.Normal },
            { "WoWI", ClientType.Installing },
        };

        private static readonly Dictionary<string, ClientLocale> _clientLocaleMapping = new Dictionary<string, ClientLocale>
        {
            { "deDE", ClientLocale.German },
            { "enCN", ClientLocale.SimplifiedChinese },
            { "enGB", ClientLocale.English },
            { "enTW", ClientLocale.TraditionalChinese },
            { "enUS", ClientLocale.English },
            { "esES", ClientLocale.Spanish },
            { "esMX", ClientLocale.MexicanSpanish },
            { "frFR", ClientLocale.French },
            { "jaJP", ClientLocale.Japanese },
            { "koKR", ClientLocale.Korean },
            { "ptBR", ClientLocale.BrazilianPortuguese },
            { "ruRU", ClientLocale.Russian },
            { "zhCN", ClientLocale.SimplifiedChinese },
            { "zhTW", ClientLocale.TraditionalChinese },
        };

        private static readonly Dictionary<string, ProcessorArchitecture> _processorMapping = new Dictionary<string, ProcessorArchitecture>
        {
            { "x86\0", ProcessorArchitecture.X86 },
        };

        private static readonly Dictionary<string, PlatformID> _platformMapping = new Dictionary<string, PlatformID>
        {
            { "Win\0", PlatformID.Win32NT },
            { "OSX\0", PlatformID.MacOSX },
        };

        public static ClientType? GetClientTypeFromFourCC(string fourCC)
        {
            Contract.Requires(fourCC != null);
            Contract.Requires(fourCC.Length == 4);

            ClientType type;
            if (_clientTypeMapping.TryGetValue(fourCC, out type))
                return type;

            return null;
        }

        public static ClientLocale? GetClientLocaleFromFourCC(string fourCC)
        {
            Contract.Requires(fourCC != null);
            Contract.Requires(fourCC.Length == 4);

            ClientLocale locale;
            if (_clientLocaleMapping.TryGetValue(fourCC, out locale))
                return locale;

            return null;
        }

        public static ProcessorArchitecture? GetProcessorFromFourCC(string fourCC)
        {
            Contract.Requires(fourCC != null);
            Contract.Requires(fourCC.Length == 4);

            ProcessorArchitecture processor;
            if (_processorMapping.TryGetValue(fourCC, out processor))
                return processor;

            return null;
        }

        public static PlatformID? GetPlatformFromFourCC(string fourCC)
        {
            Contract.Requires(fourCC != null);
            Contract.Requires(fourCC.Length == 4);

            PlatformID platform;
            if (_platformMapping.TryGetValue(fourCC, out platform))
                return platform;

            return null;
        }
    }
}
