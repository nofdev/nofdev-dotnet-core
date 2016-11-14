using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Nofdev.Core.SOA.Annotations;

namespace Nofdev.Server
{
    public class ServiceBootstrapper
    {
        public static void Scan(IEnumerable<Assembly> assemblies, Action<Type, Type> registerAction)
        {

        }

        private static void ScanByNameConvention(Assembly assembly,params string[] suffix)
        {
            
        }

        private static void Scan<T>() where T : ServiceDefinationAttribute
        {
            
        }
    }
}
