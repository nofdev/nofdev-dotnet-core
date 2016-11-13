namespace Nofdev.Core.SOA.Annotations
{
    /// <summary>
    /// Represent a service class as domain service
    /// </summary>
    public class DomainServiceAttribute : ServiceDefinationAttribute
    {
        static DomainServiceAttribute()
        {
            UrlSegment = "service";
        }
    }

}