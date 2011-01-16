using System;

namespace Trinity.Persistence
{
    [Serializable]
    public enum DatabaseType : byte
    {
        DB2,
        Firebird,
        IfxDrda,
        IfxDrda0940,
        IfxDrda1000,
        IfxOdbc,
        IfxOdbc0940,
        IfxOdbc1000,
        IfxSqli,
        IfxSqli0940,
        IfxSqli1000,
        JetDriver,
        MsSql7,
        MsSql2000,
        MsSql2005,
        MsSql2008,
        MsSqlCe,
        MySql,
        Oracle9,
        Oracle10,
        OracleData9,
        OracleData10,
        PostgreSql,
        PostgreSql81,
        PostgreSql82,
        SQLite,
    }
}
