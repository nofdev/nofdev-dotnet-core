using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Nofdev.Service.Util
{
    public class ComponentScan
    {
        private readonly ILogger<ComponentScan> _logger;
        //private readonly ServiceScanSettings _settings;

        public ComponentScan(ILogger<ComponentScan> logger)
        {
            _logger = logger;
            //_settings = settings;
            //_settings = configuration.GetSection("ServiceScanSettings")?.Get<ServiceScanSettings>();
        }
        
        private const string Ext = ".dll";

        private readonly ConcurrentDictionary<string, string[]> _fileCache =  new ConcurrentDictionary<string, string[]>();

        public IEnumerable<Assembly> GetAssemblies(string path, ServiceScanSettings settings)
        {
            var files = GetFiles(path);
            foreach (var file in files)
            {
                if (settings != null)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file);
                    if (!string.IsNullOrWhiteSpace(settings.AssemblyNameRegex) &&
                        !Regex.IsMatch(fileName, settings.AssemblyNameRegex,
                            RegexOptions.Compiled | RegexOptions.IgnoreCase))
                        continue;
                    if (settings.SkipAssemblies != null && settings.SkipAssemblies.Contains(fileName))
                        continue;

                }
                Assembly asm = null;
                try
                {

                    var name = AssemblyLoadContext.GetAssemblyName(file);
                    asm = Assembly.Load(name);
                }
                catch (Exception ex)
                {
                   _logger.LogWarning(ex,$"Load {file} failed.");
                }
                if (asm != null)
                    yield return asm;
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="attributeTypes"></param>
        /// <returns></returns>
        public Dictionary<string, Type[]> GetTypes(string path, params Type[] attributeTypes)
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


        public Dictionary<string, Type[]> GetTypes<T>(string path) where T : Attribute
        {
            var dict = new Dictionary<string, Type[]>();
            var files = GetFiles(path);
            if (files == null)
                return dict;
            foreach (var file in files)
            {
                var types = GetTypesFrom(file, typeof(T)).ToArray();
                if (types.Length > 0)
                {
                    dict.Add(file, types);
                }
            }
            return dict;
        }


        public Dictionary<string, Type[]> GetTypes(string path, string typeNameRegex)
        {
            return GetTypes(path, null, typeNameRegex);
        }

        public Dictionary<string, Type[]> GetTypes(string path, string nameSpaceRegex, string typeNameRegex)
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

        private string[] GetFiles(string path)
        {
            return _fileCache.GetOrAdd(path, SearchFiles);
        }

        private string[] SearchFiles(string path)
        {
            if (File.Exists(path))
                return new[] { path };
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
                    return new[] { path };
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

        private IEnumerable<Type> GetTypesFrom(string fileName, params Type[] attributeTypes)
        {
            var asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(fileName);
            return GetTypes(asm, attributeTypes);
        }

        public IEnumerable<Type> GetTypes<T>(Assembly asm) where T : Attribute
        {
            return GetTypes(asm, typeof(T));
        }

        public IEnumerable<Type> GetTypes(Assembly asm, params Type[] attributeTypes)
        {
            return
                asm.GetTypes()
                    .Where(
                        t =>
                            t.GetTypeInfo()
                                .GetCustomAttributes(true)
                                .Any(a => attributeTypes.Any(ta => ta == a.GetType())));
        }

        private IEnumerable<Type> GetTypesFrom(string fileName, string nameSpaceRegex, string typeNameRegex)
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