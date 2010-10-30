using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Trinity.Encore.Framework.Core.Reflection
{
    public static class ReflectionUtilities
    {
        public static int GetEnumValueCount<T>()
        {
            Contract.Assume(typeof(T).IsEnum);

            return Enum.GetValues(typeof(T)).Length;
        }

        public static T GetMaxEnumValue<T>()
        {
            Contract.Assume(typeof(T).IsEnum);

            return ((T[])Enum.GetValues(typeof(T))).Max();
        }

        public static MethodInfo MethodOf(Expression<Action> expr)
        {
            var body = expr.Body as MethodCallExpression;

            if (body == null)
                throw new ArgumentException("Expression is not a method call.", "expr");

            return body.Method;
        }

        public static ConstructorInfo ConstructorOf<T>(Expression<Func<T>> expr)
        {
            var body = expr.Body as NewExpression;

            if (body == null)
                throw new ArgumentException("Expression is not an object construction operation.", "expr");

            return body.Constructor;
        }

        public static PropertyInfo PropertyOf<T, TResult>(Expression<Func<T, TResult>> expr)
        {
            var body = expr.Body as MemberExpression;

            if (body == null)
                throw new ArgumentException("Expression must be a member expression.");

            var member = body.Member as PropertyInfo;

            if (member == null)
                throw new ArgumentException("Member expression is not a property.", "expr");

            return member;
        }

        public static FieldInfo FieldOf<T, TResult>(Expression<Func<T, TResult>> expr)
        {
            var body = expr.Body as MemberExpression;

            if (body == null)
                throw new ArgumentException("Expression must be a member expression.");

            var member = body.Member as FieldInfo;

            if (member == null)
                throw new ArgumentException("Member expression is not a field.", "expr");

            return member;
        }

        public static EventInfo EventOf<T>(Expression<Func<T>> expr)
        {
            var body = expr.Body as MemberExpression;

            if (body == null)
                throw new ArgumentException("Expression must be a member expression.");

            var member = body.Member as EventInfo;

            if (member == null)
                throw new ArgumentException("Member expression is not an event.", "expr");

            return member;
        }
    }
}
