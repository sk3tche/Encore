using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Trinity.Encore.Framework.Core.Runtime
{
    public static class HashCodeUtility
    {
        private const int HashPrime1 = 17;

        private const int HashPrime2 = 23;

        public static int GetHashCode<T>(params T[] fields)
            where T : IEquatable<T>
        {
            Contract.Requires(fields != null);

            unchecked
            {
                return fields.Aggregate(HashPrime1, (acc, field) =>
                {
                    // Do not try to simplify this line. It has to be like this to avoid boxing.
                    var fieldHash = field.Equals(default(T)) ? 0.GetHashCode() : field.GetHashCode();
                    return HashPrime2 * acc + fieldHash;
                });
            }
        }

        public static int GetHashCode<T1, T2>(T1 t1, T2 t2)
            where T1 : IEquatable<T1>
            where T2 : IEquatable<T2>
        {
            return GetHashCode(t1) * GetHashCode(t2);
        }

        public static int GetHashCode<T1, T2, T3>(T1 t1, T2 t2, T3 t3)
            where T1 : IEquatable<T1>
            where T2 : IEquatable<T2>
            where T3 : IEquatable<T3>
        {
            return GetHashCode(t1) * GetHashCode(t2) * GetHashCode(t3);
        }

        public static int GetHashCode<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4)
            where T1 : IEquatable<T1>
            where T2 : IEquatable<T2>
            where T3 : IEquatable<T3>
            where T4 : IEquatable<T4>
        {
            return GetHashCode(t1) * GetHashCode(t2) * GetHashCode(t3) * GetHashCode(t4);
        }

        public static int GetHashCode<T1, T2, T3, T4, T5>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
            where T1 : IEquatable<T1>
            where T2 : IEquatable<T2>
            where T3 : IEquatable<T3>
            where T4 : IEquatable<T4>
            where T5 : IEquatable<T5>
        {
            return GetHashCode(t1) * GetHashCode(t2) * GetHashCode(t3) * GetHashCode(t4) * GetHashCode(t5);
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
            where T1 : IEquatable<T1>
            where T2 : IEquatable<T2>
            where T3 : IEquatable<T3>
            where T4 : IEquatable<T4>
            where T5 : IEquatable<T5>
            where T6 : IEquatable<T6>
        {
            return GetHashCode(t1) * GetHashCode(t2) * GetHashCode(t3) * GetHashCode(t4) * GetHashCode(t5) * GetHashCode(t6);
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
            where T1 : IEquatable<T1>
            where T2 : IEquatable<T2>
            where T3 : IEquatable<T3>
            where T4 : IEquatable<T4>
            where T5 : IEquatable<T5>
            where T6 : IEquatable<T6>
            where T7 : IEquatable<T7>
        {
            return GetHashCode(t1) * GetHashCode(t2) * GetHashCode(t3) * GetHashCode(t4) * GetHashCode(t5) * GetHashCode(t6) *
                GetHashCode(t7);
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7, T8>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
            where T1 : IEquatable<T1>
            where T2 : IEquatable<T2>
            where T3 : IEquatable<T3>
            where T4 : IEquatable<T4>
            where T5 : IEquatable<T5>
            where T6 : IEquatable<T6>
            where T7 : IEquatable<T7>
            where T8 : IEquatable<T8>
        {
            return GetHashCode(t1) * GetHashCode(t2) * GetHashCode(t3) * GetHashCode(t4) * GetHashCode(t5) * GetHashCode(t6) *
                GetHashCode(t7) * GetHashCode(t8);
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7,
            T8 t8, T9 t9)
            where T1 : IEquatable<T1>
            where T2 : IEquatable<T2>
            where T3 : IEquatable<T3>
            where T4 : IEquatable<T4>
            where T5 : IEquatable<T5>
            where T6 : IEquatable<T6>
            where T7 : IEquatable<T7>
            where T8 : IEquatable<T8>
            where T9 : IEquatable<T9>
        {
            return GetHashCode(t1) * GetHashCode(t2) * GetHashCode(t3) * GetHashCode(t4) * GetHashCode(t5) * GetHashCode(t6) *
                GetHashCode(t7) * GetHashCode(t8) * GetHashCode(t9);
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7,
            T8 t8, T9 t9, T10 t10)
            where T1 : IEquatable<T1>
            where T2 : IEquatable<T2>
            where T3 : IEquatable<T3>
            where T4 : IEquatable<T4>
            where T5 : IEquatable<T5>
            where T6 : IEquatable<T6>
            where T7 : IEquatable<T7>
            where T8 : IEquatable<T8>
            where T9 : IEquatable<T9>
            where T10 : IEquatable<T10>
        {
            return GetHashCode(t1) * GetHashCode(t2) * GetHashCode(t3) * GetHashCode(t4) * GetHashCode(t5) * GetHashCode(t6) *
                GetHashCode(t7) * GetHashCode(t8) * GetHashCode(t9) * GetHashCode(t10);
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6,
            T7 t7, T8 t8, T9 t9, T10 t10, T11 t11)
            where T1 : IEquatable<T1>
            where T2 : IEquatable<T2>
            where T3 : IEquatable<T3>
            where T4 : IEquatable<T4>
            where T5 : IEquatable<T5>
            where T6 : IEquatable<T6>
            where T7 : IEquatable<T7>
            where T8 : IEquatable<T8>
            where T9 : IEquatable<T9>
            where T10 : IEquatable<T10>
            where T11 : IEquatable<T11>
        {
            return GetHashCode(t1) * GetHashCode(t2) * GetHashCode(t3) * GetHashCode(t4) * GetHashCode(t5) * GetHashCode(t6) *
                GetHashCode(t7) * GetHashCode(t8) * GetHashCode(t9) * GetHashCode(t10) * GetHashCode(t11);
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5,
            T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12)
            where T1 : IEquatable<T1>
            where T2 : IEquatable<T2>
            where T3 : IEquatable<T3>
            where T4 : IEquatable<T4>
            where T5 : IEquatable<T5>
            where T6 : IEquatable<T6>
            where T7 : IEquatable<T7>
            where T8 : IEquatable<T8>
            where T9 : IEquatable<T9>
            where T10 : IEquatable<T10>
            where T11 : IEquatable<T11>
            where T12 : IEquatable<T12>
        {
            return GetHashCode(t1) * GetHashCode(t2) * GetHashCode(t3) * GetHashCode(t4) * GetHashCode(t5) * GetHashCode(t6) *
                GetHashCode(t7) * GetHashCode(t8) * GetHashCode(t9) * GetHashCode(t10) * GetHashCode(t11) * GetHashCode(t12);
        }
    }
}
