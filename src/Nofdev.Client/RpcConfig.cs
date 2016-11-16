namespace Nofdev.Client
{
    public class RpcConfig
    {
        /// <summary>
        /// rpc.json
        /// </summary>
        public const string FileName = "rpc.json";

        public string Company { get; set; }

        public string Version { get; set; }

        public ServiceLocation[] Services { get; set; }
    }

    public class ServiceLocation
    {
        public string Namespace { get; set; }
        public string BaseUrl { get; set; }
        public string Protocol { get; set; }
        internal string Layer { get; set; }
    }
}
