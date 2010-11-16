using FluentNHibernate.Mapping;

namespace Trinity.Encore.Framework.Persistence
{
    /// <summary>
    /// Wrapper/abstraction class that covers FluentNHibernate.Mapping.ClassMap
    /// </summary>
    /// <typeparam name="TRecord">The representation of the mappable record class.</typeparam>
    public abstract class MappableObject<TRecord> : ClassMap<TRecord>
        where TRecord : class
    {
    }
}
