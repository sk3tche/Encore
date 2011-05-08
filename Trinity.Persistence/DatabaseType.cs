using System;

namespace Trinity.Persistence
{
    [Serializable]
    public enum DatabaseType : byte
    {
        DB2,
        MsSql2005,
        MsSql2008,
        MsSqlCe,
        MySql,
        Oracle10,
        OracleData10,
        PostgreSql,
        SQLite,
    }
}
