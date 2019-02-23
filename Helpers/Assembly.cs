using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SVN.Reflection.Helpers
{
    public static class Assembly
    {
        public static System.Reflection.Assembly GetCallingAssembly(int depth = 0)
        {
            depth++;

            var stackTrace = new StackTrace();
            var frames = stackTrace.GetFrames();
            var method = frames[depth].GetMethod();
            var assembly = method.DeclaringType.Assembly;

            return assembly;
        }

        public static string GetCallingAssemblyName(int depth = 0)
        {
            depth++;

            var stackTrace = new StackTrace();
            var frames = stackTrace.GetFrames();
            var method = frames[depth].GetMethod();
            var assembly = method.DeclaringType.Assembly;
            var assemblyName = assembly.GetName();

            return assemblyName.Name;
        }

        public static string GetCallingAssemblyVersion(int depth = 0)
        {
            depth++;

            var stackTrace = new StackTrace();
            var frames = stackTrace.GetFrames();
            var method = frames[depth].GetMethod();
            var assembly = method.DeclaringType.Assembly;
            var assemblyName = assembly.GetName();
            var assemblyVersion = assemblyName.Version;

            return assemblyVersion.ToString();
        }

        public static string GetResource(params string[] endsWith)
        {
            var assemblyName = Assembly.GetCallingAssemblyName(1);

            foreach (var stream in Assembly.GetResources(assemblyName, endsWith).Select(x => x.stream))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }

            return string.Empty;
        }

        public static IEnumerable<(string filename, Stream stream)> GetResources(string assemblyName, string startsWith)
        {
            var assembly = System.Reflection.Assembly.Load(assemblyName);
            var resources = assembly.GetManifestResourceNames();

            foreach (var resource in resources.Where(x => x.StartsWith($"{assemblyName}.{startsWith}")))
            {
                var filename = resource;

                while (2 <= filename.Count(x => x == '.'))
                {
                    filename = filename.Substring(filename.IndexOf('.') + 1);
                }

                yield return (filename, assembly.GetManifestResourceStream(resource));
            }
        }

        public static IEnumerable<(string filename, Stream stream)> GetResources(string assemblyName, params string[] endsWith)
        {
            var assembly = System.Reflection.Assembly.Load(assemblyName);
            var resources = assembly.GetManifestResourceNames();

            foreach (var resource in resources.Where(x => endsWith.Any(y => x.EndsWith(y))))
            {
                var filename = resource;

                while (2 <= filename.Count(x => x == '.'))
                {
                    filename = filename.Substring(filename.IndexOf('.') + 1);
                }

                yield return (filename, assembly.GetManifestResourceStream(resource));
            }
        }
    }
}