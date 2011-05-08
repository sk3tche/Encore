using System;
using System.Collections.Generic;

namespace Trinity.Persistence.Versioning
{
    internal sealed class NullLogger : global::Migrator.Framework.ILogger
    {
        public void Started(List<long> currentVersion, long finalVersion)
        {
        }

        public void MigrateUp(long version, string migrationName)
        {
        }

        public void MigrateDown(long version, string migrationName)
        {
        }

        public void Skipping(long version)
        {
        }

        public void RollingBack(long originalVersion)
        {
        }

        public void ApplyingDBChange(string sql)
        {
        }

        public void Log(string format, params object[] args)
        {
        }

        public void Warn(string format, params object[] args)
        {
        }

        public void Trace(string format, params object[] args)
        {
        }

        public void Finished(List<long> currentVersion, long finalVersion)
        {
        }

        public void Exception(string message, Exception ex)
        {
        }

        public void Exception(long version, string migrationName, Exception ex)
        {
        }
    }
}
