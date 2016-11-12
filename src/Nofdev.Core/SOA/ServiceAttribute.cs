using System;

namespace Nofdev.Core.SOA
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ServiceAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        protected ServiceAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; protected set; }
    }
}
