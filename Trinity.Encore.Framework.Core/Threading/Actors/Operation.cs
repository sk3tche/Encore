using System;

namespace Trinity.Encore.Framework.Core.Threading.Actors
{
    [Serializable]
    public enum Operation : byte
    {
        None,
        Continue,
        Dispose,
    }
}
