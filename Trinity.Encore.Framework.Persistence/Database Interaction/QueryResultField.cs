using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trinity.Encore.Framework.Persistence
{
    class QueryResultField
    {
        /// <summary>
        /// Get value for the field.
        /// </summary>
        /// <returns>8-bit byte (TINYINT unsigned) format</returns>
        public byte GetByte()
        {
            return 0;
        }

        /// <summary>
        /// Get value for the field.
        /// </summary>
        /// <returns>8-bit sbyte (TINYINT signed) format</returns>
        public sbyte GetSByte()
        {
            return 0;
        }

        /// <summary>
        /// Get value for the field.
        /// </summary>
        /// <returns>16-bit UInt16 (SMALLINT unsigned) format</returns>
        public UInt16 GetUInt16()
        {
            return 0;
        }

        /// <summary>
        /// Get value for the field.
        /// </summary>
        /// <returns>16-bit Int16 (SMALLINT signed) format</returns>
        public Int16 GetInt16()
        {
            return 0;
        }

        /// <summary>
        /// Get value for the field.
        /// </summary>
        /// <returns>32-bit UInt32 (INT unsigned) format</returns>
        public UInt32 GetUInt32()
        {
            return 0;
        }

        /// <summary>
        /// Get value for the field.
        /// </summary>
        /// <returns>32-bit Int32 (INT signed) format</returns>
        public Int32 GetInt32()
        {
            return 0;
        }

        /// <summary>
        /// Get value for the field.
        /// </summary>
        /// <returns>32-bit float (FLOAT) format</returns>
        public float GetFloat()
        {
            return 0.0f;
        }

        /// <summary>
        /// Get value for the field.
        /// </summary>
        /// <returns>64-bit UInt64 (BIGINT unsigned) format</returns>
        public UInt64 GetUInt64()
        {
            return 0;
        }

        /// <summary>
        /// Get value for the field.
        /// </summary>
        /// <returns>64-bit Int64 (BIGINT signed) format</returns>
        public Int64 GetInt64()
        {
            return 0;
        }

        /// <summary>
        /// Get value for the field.
        /// </summary>
        /// <returns>64-bit double (DOUBLE) format</returns>
        public double GetDouble()
        {
            return 0.0f;
        }

        /// <summary>
        /// Get value for the field.
        /// </summary>
        /// <returns>string (VARCHAR, TEXT, BLOB) format</returns>
        public string GetString()
        {
            return "";
        }
    }
}
