using System;

namespace Nofdev.Core.SOA
{
    /// <summary>
    /// Represent a service class as domain service
    /// </summary>
    public class DomainServiceAttribute : Attribute
    {
        /// <summary>
        /// Url segment,default is "service"
        /// </summary>
        public const string UrlSegment = "service";
    }

}