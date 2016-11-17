using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;

namespace Nofdev.Core.Util
{
    public static class ComponentScan
    {
        private const string Ext = ".dll";

        private static readonly ConcurrentDictionary<string, string[]> _fileCache =
            new ConcurrentDictionary<string, string[]>();


        public static IEnumerable<Assembly> GetAssemblies(string path)
        {
            var files = GetFiles(path);
            foreach (var file in files)
            {
                var name = AssemblyLoadContext.GetAssemblyName(file);
                yield return Assembly.Load(name);
            }
            //return files.Select(file =>  AssemblyLoadContext.Default.LoadFromAssemblyPath(file));
        }


        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="attributeTypes"></param>
        /// <returns></returns>
        public static Dictionary<string, Type[]> GetTypes(string path, params Type[] attributeTypes)
        {
            var dict = new Dictionary<string, Type[]>();
            var files = GetFiles(path);
            foreach (var file in files)
            {
                var types = GetTypesFrom(file, attributeTypes).ToArray();
                if (types.Length > 0)
                {
                    dict.Add(file, types);
                }
            }
            return dict;
        }


        public static Dictionary<string, Type[]> GetTypes<T>(string path) where T : Attribute
        {
            var dict = new Dictionary<string, Type[]>();
            var files = GetFiles(path);
            if (files == null)
                return dict;
            foreach (var file in files)
            {
                var types = GetTypesFrom(file, typeof (T)).ToArray();
                if (types.Length > 0)
                {
                    dict.Add(file, types);
                }
            }
            return dict;
        }


        public static Dictionary<string, Type[]> GetTypes(string path, string typeNameRegex)
        {
            return GetTypes(path, null, typeNameRegex);
        }

        public static Dictionary<string, Type[]> GetTypes(string path, string nameSpaceRegex, string typeNameRegex)
        {
            var dict = new Dictionary<string, Type[]>();
            var files = GetFiles(path);
            if (files == null)
                return dict;
            foreach (var file in files)
            {
                var types = GetTypesFrom(file, nameSpaceRegex, typeNameRegex).ToArray();
                if (types.Length > 0)
                {
                    dict.Add(file, types);
                }
            }
            return dict;
        }

        private static string[] GetFiles(string path)
        {
            return _fileCache.GetOrAdd(path, SearchFiles);
        }

        private static string[] SearchFiles(string path)
        {
            if (File.Exists(path))
                return new[] {path};
            path = path.Replace("/", "\\");
            if (!path.Contains("*") && !path.Contains("?"))
            {
                if (path.EndsWith(Ext))
                {
                    var file = new FileInfo(path);
                    if (!file.Exists)
                    {
                        return null;
                    }
                    return new[] {path};
                }

                var dir = new DirectoryInfo(path);
                if (!dir.Exists)
                {
                    return null;
                }
                return dir.GetFiles("*" + Ext, SearchOption.AllDirectories).Select(m => m.FullName).ToArray();
            }


            var folder = path.Substring(0, path.LastIndexOf("\\", StringComparison.Ordinal) + 1);
            var pattern = path.Substring(path.LastIndexOf("\\", StringComparison.Ordinal) + 1,
                path.Length - path.LastIndexOf("\\", StringComparison.Ordinal) - 1);
            if (pattern.EndsWith("*") || pattern.EndsWith("?"))
                pattern += Ext;
            else if (!pattern.EndsWith(Ext))
                pattern += "*" + Ext;


            var dir2 = new DirectoryInfo(folder);
            return !dir2.Exists
                ? null
                : dir2.GetFiles(pattern, SearchOption.AllDirectories).Select(m => m.FullName).ToArray();
        }

        private static IEnumerable<Type> GetTypesFrom(string fileName, params Type[] attributeTypes)
        {
            var asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(fileName);
            return asm.GetTypes(attributeTypes);
        }

        public static IEnumerable<Type> GetTypes<T>(this Assembly asm) where T : Attribute
        {
            return asm.GetTypes(typeof (T));
        }

        public static IEnumerable<Type> GetTypes(this Assembly asm, params Type[] attributeTypes)
        {
            return
                asm.GetTypes()
                    .Where(
                        t =>
                            t.GetTypeInfo()
                                .GetCustomAttributes(true)
                                .Any(a => attributeTypes.Any(ta => ta == a.GetType())));
        }

        private static IEnumerable<Type> GetTypesFrom(string fileName, string nameSpaceRegex, string typeNameRegex)
        {
            var asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(fileName);
            var types = asm.GetTypes();
            foreach (var type in types)
            {
                if ((string.IsNullOrWhiteSpace(nameSpaceRegex) ||
                     Regex.IsMatch(type.FullName, nameSpaceRegex, RegexOptions.IgnoreCase | RegexOptions.Compiled)) &&
                    Regex.IsMatch(type.Name, typeNameRegex, RegexOptions.IgnoreCase | RegexOptions.Compiled))
                    yield return type;
            }
        }


    }
}