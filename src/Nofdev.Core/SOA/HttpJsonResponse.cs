namespace Nofdev.Core.SOA
{
    /// <summary>
    /// HTTP JSON response based on Nof-dev specification
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HttpJsonResponse<T>
    {
        /// <summary>
        /// value
        /// </summary>
        public T val { get; set; }

        /// <summary>
        /// call ID that client side sent
        /// </summary>
        public string callId { get; set; }

        /// <summary>
        /// error
        /// </summary>
        public ExceptionMessage err { get; set; }
    }
}