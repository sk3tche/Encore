using System.Diagnostics.Contracts;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Trinity.Encore.Framework.Game.Database.Conventions.Naming
{
    public sealed class ClassNameConvention : IClassConvention
    {
        public const string Record = "Record";

        public void Apply(IClassInstance instance)
        {
            var tableName = instance.TableName;
            var recIndex = tableName.IndexOf(Record);

            if (recIndex == -1)
                return;

            var recLength = Record.Length;
            Contract.Assume(recIndex + recLength < tableName.Length);
            instance.Table(tableName.Remove(recIndex, recLength));
        }
    }
}
