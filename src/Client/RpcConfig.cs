namespace Nofdev.Client
{
    public class RpcConfig
    {
        /// <summary>
        /// rpc.json
        /// </summary>
        public const string FileName = "rpc.json";

        public string Version { get; set; } = "1.0";

        public ServiceLocation[] Services { get; set; }
    }

    public class ServiceLocation
    {
        private string _typeNameScan ;
        public string AssemblyString { get; set; }
        public string Namespace { get; set; }

        public string TypeNameScan
        {
            get => _typeNameScan ?? Layer;
            set => _typeNameScan = value;
        }

        public string BaseUrl { get; set; }
        public string Protocol { get; set; }
        public string ProxyStrategy { get; set; }
        public string Layer { get; set; }

        public string AssemblyName
        {
            get
            {
                var name = AssemblyString;
                if (name.Contains(","))
                    name = name.Split(',')[0].Trim();
                return name;
            }
        }
    }
}
