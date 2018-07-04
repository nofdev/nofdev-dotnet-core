using System;
using System.Collections.Generic;

namespace Nofdev.Service
{
    public class ServiceScanOptions
    {
        public string[] RpcAssemblyFiles { get; set; }

        public ServiceModuleOptions[] ServiceModules { get; set; }
    }

    public class ServiceModuleOptions
    {
        public string Layer { get; set; }
        public AssemblyOptions[] Dependencies { get; set; }
        public string EntryAssemblyFile { get; set; }
    }

    public class AssemblyOptions
    {
        public string AssemblyFile { get; set; }
        public string NameRegex { get; set; }
        public Dictionary<Type,Type> DependentTypes { get; set; }
    }
}