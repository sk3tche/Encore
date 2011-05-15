using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using JetBrains.Annotations;
using Trinity.Core.Reflection;

namespace Trinity.Core
{
    public static class Extensions
    {
        /// <summary>
        /// Casts one thing into another.
        /// </summary>
        /// <remarks>
        /// This is a hack. It should only be used in rare cases.
        /// </remarks>
        public static T CastType<T>(this object obj)
        {
            Contract.Requires(obj != null);
            Contract.Ensures(Contract.Result<T>() != null);

            var value = (T)Cast(obj, typeof(T));
            Contract.Assume(value != null);
            return value;
        }

        /// <summary>
        /// Casts one thing into another.
        /// </summary>
        /// <remarks>
        /// This is a hack. It should only be used in rare cases.
        /// </remarks>
        public static object Cast(this object obj, Type newType)
        {
            Contract.Requires(obj != null);
            Contract.Requires(newType != null);
            Contract.Ensures(Contract.Result<object>() != null);

            if (newType.IsEnum)
            {
                var str = obj as string;
                return str != null ? Enum.Parse(newType, str) : Enum.ToObject(newType, obj);
            }

            var type = obj.GetType();
            if (type.IsInteger() && newType == typeof(bool)) // A hack for boolean values.
                return obj.Equals(0.Cast(type)) ? false : true;

            // Since we require that value is not null, the returned value won't be either.
            var value = Convert.ChangeType(obj, newType, CultureInfo.InvariantCulture);
            Contract.Assume(value != null);
            return value;
        }

        [CLSCompliant(false)]
        public static IConvertible AsConvertible<T>(this T value)
            where T : IConvertible
        {
            return value;
        }

        /// <summary>
        /// Checks if a given enum value has any of the given enum flags.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="toTest">The flags to test.</param>
        public static bool HasAnyFlag(this Enum value, Enum toTest)
        {
            Contract.Requires(value != null);
            Contract.Requires(toTest != null);

            var val = ((IConvertible)value).ToUInt64(null);
            var test = ((IConvertible)toTest).ToUInt64(null);

            return (val & test) != 0;
        }

        /// <summary>
        /// Checks if an enum value is valid (that is, defined in the enumeration type).
        /// </summary>
        /// <param name="value">The value to check.</param>
        [Pure]
        public static bool IsValid(this Enum value)
        {
            Contract.Requires(value != null);

            return Enum.IsDefined(value.GetType(), value);
        }

        /// <summary>
        /// Ensures that a given enum value is valid (defined). Throws if not.
        /// </summary>
        /// <param name="value">The value to check.</param>
        public static Enum EnsureValid(this Enum value)
        {
            Contract.Requires(value != null);
            Contract.Ensures(Contract.Result<Enum>() != null);

            if (!value.IsValid())
                throw new ArgumentException("Enum value is not valid.");

            return value;
        }

        /// <summary>
        /// Checks if an IntPtr object is null (0).
        /// </summary>
        /// <param name="ptr">The IntPtr object.</param>
        public static bool Null(this IntPtr ptr)
        {
            return ptr == IntPtr.Zero;
        }

        /// <summary>
        /// Checks if an UIntPtr object is null (0).
        /// </summary>
        /// <param name="ptr">The IntPtr object.</param>
        [CLSCompliant(false)]
        public static bool Null(this UIntPtr ptr)
        {
            return ptr == UIntPtr.Zero;
        }

        public static string ToHexString(this IntPtr pointer)
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            var stringRep = "0x" + pointer.ToString("X");
            Contract.Assume(!string.IsNullOrEmpty(stringRep));
            return stringRep;
        }

        [StringFormatMethod("str")]
        public static string Interpolate(this string str, params object[] args)
        {
            Contract.Requires(str != null);
            Contract.Requires(args != null);
            Contract.Ensures(Contract.Result<string>() != null);

            return string.Format(CultureInfo.InvariantCulture, str, args);
        }

        public static bool IsBetween<T>(this T comparable, T lower, T upper)
            where T : IComparable<T>
        {
            Contract.Requires(comparable != null);
            Contract.Requires(lower != null);
            Contract.Requires(upper != null);

            return comparable.CompareTo(lower) >= 0 && comparable.CompareTo(upper) < 0;
        }

        public static T With<T>(this T obj, Action<T> act)
            where T : class
        {
            Contract.Requires(obj != null);
            Contract.Requires(act != null);
            Contract.Ensures(Contract.Result<T>() != null);

            act(obj);
            return obj;
        }
    }
}
