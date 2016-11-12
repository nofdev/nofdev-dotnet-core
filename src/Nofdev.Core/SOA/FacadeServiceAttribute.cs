using System;

namespace Nofdev.Core.SOA
{
    /// <summary>
    /// Represent a service class as facade service
    /// </summary>
    public class FacadeServiceAttribute : Attribute
    {
        /// <summary>
        /// Url segment,default is "facade"
        /// </summary>
        public const string UrlSegment = "facade";
    }
}
 