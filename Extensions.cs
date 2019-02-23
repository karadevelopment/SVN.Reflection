using System;
using System.Reflection;

namespace SVN.Reflection
{
    public static class Extensions
    {
        public static string GetClassName(this Type param)
        {
            return param.Name;
        }

        public static string GetClassName<T>(this T param)
        {
            return param.GetType().GetClassName();
        }

        public static string GetNamespace(this Type param)
        {
            return param.Namespace;
        }

        public static string GetNamespace<T>(this T param)
        {
            return param.GetType().GetNamespace();
        }

        public static object GetDefault(this Type param)
        {
            if (param.IsValueType)
            {
                return Activator.CreateInstance(param);
            }
            return null;
        }

        public static T GetValue<T>(this object param, string propertyName)
        {
            var bindingFlags = (BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            var type = param?.GetType();
            var property = type?.GetProperty(propertyName, bindingFlags);
            var value = property?.GetValue(param, null);

            return (T)(value ?? default(T));
        }

        public static void With<T>(this T param, Action<T> callback)
        {
            callback(param);
        }

        public static T With<T>(this T param, Func<T, T> callback)
        {
            return callback(param);
        }
    }
}