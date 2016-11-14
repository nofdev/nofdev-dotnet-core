using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nofdev.Server
{
    public class DependencyBootstrapper
    {
        public static void Scan(IEnumerable<Assembly> assemblies,Action<Type,Type> registerAction)
        {
            var interfaceList = new List<Type>();
            var assemblyList = assemblies.ToList();

            assemblyList.ForEach(
                    a => interfaceList.AddRange(a.GetTypes().Where(t => t.GetTypeInfo().IsInterface || t.GetTypeInfo().IsAbstract)));
            assemblyList.ForEach(a =>
            {
                var types = a.GetTypes().Where(t => t.GetTypeInfo().IsClass).ToList();
                types.ForEach(t =>
                {
                    var interfaces = t.GetInterfaces().ToList();
                    interfaces.ForEach(i =>
                    {
                        foreach (var type in interfaceList.Where(m => m == i))
                        {
                                registerAction(type, t);
                                interfaceList.Remove(type);
                                break;
                        }
                    });
                });
            });
        }
    }
}
