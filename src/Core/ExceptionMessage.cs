using System;

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


        public static  ExceptionMessage FromException(Exception exception,bool enableStackTrace)
        {
            if (exception == null) return null;
            var exceptionMessage = new ExceptionMessage
            {
                Name = exception.GetType().Name,
                Msg = exception.Message,
                Cause = FromException(exception.InnerException, enableStackTrace)
            };
            if (enableStackTrace)
            {
                exceptionMessage.Stack = exception.StackTrace;
            }
            return exceptionMessage;
        }
    }
}