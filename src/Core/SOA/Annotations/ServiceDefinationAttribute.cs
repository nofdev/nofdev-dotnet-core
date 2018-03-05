using System;

namespace Nofdev.Core.SOA.Annotations
{
    /// <summary>
    /// ServiceDefinationAttribute
    /// </summary>
    public abstract class ServiceDefinationAttribute : Attribute
    {
        public static string UrlSegment { get; protected set; }
    }
}