namespace Nofdev.Core.SOA
{
    /// <summary>
    /// A simple HTTP message object
    /// </summary>
    public class HttpMessageSimple
    {
        /// <summary>
        /// StatusCode,e.g. 200/404
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// ContentType,e.g. Plain/HTML
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Body
        /// </summary>
        public string Body { get; set; }
    }
}
