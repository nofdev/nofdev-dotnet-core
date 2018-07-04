using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Nofdev.Bootstrapper.AutoMapperExt
{
    public class AutoProfileLoader
    {
        public static List<Assembly> Assemblies => new List<Assembly>();

        public static AutoProfile Load(IEnumerable<MappingAssemblyPair> assemblyPairs)
        {
            var root = AppContext.BaseDirectory;
            var profile = new AutoProfile();
            foreach (var pair in assemblyPairs)
            {
                var dtoAsm = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(root, pair.SourceAssembyFile));
                var entAsm =
                    AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(root, pair.DestinationAssemblyFile));
                profile.Configure(dtoAsm,entAsm);

                if(Assemblies.All(a => a.FullName != dtoAsm.FullName))
                    Assemblies.Add(dtoAsm);

                if (Assemblies.All(a => a.FullName != entAsm.FullName))
                    Assemblies.Add(entAsm);
            }

            return profile;
        }

    }
}
