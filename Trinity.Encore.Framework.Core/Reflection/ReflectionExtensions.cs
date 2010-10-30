using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using Trinity.Encore.Framework.Core.Collections;

namespace Trinity.Encore.Framework.Core.Reflection
{
    public static class ReflectionExtensions
    {
        [Pure]
        public static T[] GetCustomAttributes<T>(this Type type)
            where T : Attribute
        {
            Contract.Requires(type != null);
            Contract.Ensures(Contract.Result<T[]>() != null);

            var attribs = type.GetCustomAttributes(typeof(T), false) as T[];
            Contract.Assume(attribs != null);
            return attribs;
        }

        [Pure]
        public static T[] GetCustomAttributes<T>(this MemberInfo member)
            where T : Attribute
        {
            Contract.Requires(member != null);
            Contract.Ensures(Contract.Result<T[]>() != null);

            var attribs = member.GetCustomAttributes(typeof(T), false) as T[];
            Contract.Assume(attribs != null);
            return attribs;
        }

        [Pure]
        public static T GetCustomAttribute<T>(this Type type)
            where T : Attribute
        {
            Contract.Requires(type != null);

            return type.GetCustomAttributes<T>().TryGet(0);
        }

        [Pure]
        public static T GetCustomAttribute<T>(this MemberInfo member)
            where T : Attribute
        {
            Contract.Requires(member != null);

            return member.GetCustomAttributes<T>().TryGet(0);
        }

        [Pure]
        public static bool Implements<T>(this Type type, bool mainType = true)
        {
            Contract.Requires(type != null);

            var otherType = typeof(T);
            if (mainType && type == otherType)
                return true;

            return type.BaseType == otherType || type.GetInterfaces().Any(impl => impl == otherType);
        }

        /// <summary>
        /// Checks if the type is a simple type.
        /// 
        /// Simple types are primitive types and strings.
        /// </summary>
        [Pure]
        public static bool IsSimpleType(this Type type)
        {
            Contract.Requires(type != null);

            return type.IsEnum || type.IsNumericType() || type == typeof(string) || type == typeof(char) ||
                type == typeof(bool);
        }

        [Pure]
        public static bool IsNumericType(this Type type)
        {
            Contract.Requires(type != null);

            return type.IsInteger() || type.IsFloatingPoint();
        }

        [Pure]
        public static bool IsFloatingPoint(this Type type)
        {
            Contract.Requires(type != null);

            return type == typeof(float) || type == typeof(double) || type == typeof(decimal);
        }

        [Pure]
        public static bool IsInteger(this Type type)
        {
            Contract.Requires(type != null);

            return type == typeof(int) || type == typeof(uint) || type == typeof(short) || type == typeof(ushort) ||
                type == typeof(byte) || type == typeof(sbyte) || type == typeof(long) || type == typeof(ulong);
        }

        public static T ForgeDelegate<T>(this MethodInfo method)
            where T : class
        {
            Contract.Requires(method != null);
            Contract.Assume(typeof(T).Implements<MulticastDelegate>(false));

            return Delegate.CreateDelegate(typeof(T), method) as T;
        }

        public static void SetUnindexedValue(this MemberInfo member, object obj, object value)
        {
            Contract.Requires(member != null);
            Contract.Assume(member is FieldInfo || member is PropertyInfo);

            var fieldInfo = member as FieldInfo;
            if (fieldInfo != null)
                fieldInfo.SetValue(obj, value);
            else
                ((PropertyInfo)member).SetValue(obj, value, null);
        }

        public static object GetUnindexedValue(this MemberInfo member, object obj)
        {
            Contract.Requires(member != null);
            Contract.Assume(member is FieldInfo || member is PropertyInfo);

            var fieldInfo = member as FieldInfo;
            return fieldInfo != null ? fieldInfo.GetValue(obj) : ((PropertyInfo)member).GetValue(obj, null);
        }
    }
}
