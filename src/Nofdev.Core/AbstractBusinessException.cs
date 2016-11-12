using System;

namespace Nofdev.Core
{
    /// <summary>
    /// AbstractBusinessException
    /// </summary>
    public abstract class AbstractBusinessException : Exception
    {
        /// <summary>
        /// constructor
        /// </summary>
        protected AbstractBusinessException()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        protected AbstractBusinessException(string message) : base(message)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        protected AbstractBusinessException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}