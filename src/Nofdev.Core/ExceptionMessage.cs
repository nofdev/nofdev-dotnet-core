namespace Nofdev.Core
{
    /// <summary>
    /// ExceptionMessage
    /// </summary>
    public class ExceptionMessage
    {
        /// <summary>
        /// name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// message
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// cause
        /// </summary>
        public ExceptionMessage Cause { get; set; }

        /// <summary>
        /// stack trace
        /// </summary>
        public string Stack { get; set; }
    }
}