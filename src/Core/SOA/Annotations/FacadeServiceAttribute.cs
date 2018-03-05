namespace Nofdev.Core.SOA.Annotations
{
    /// <summary>
    /// Represent a service class as facade service
    /// </summary>
    public class FacadeServiceAttribute : ServiceDefinationAttribute
    {
        static FacadeServiceAttribute()
        {
            UrlSegment = "facade";
        }
    }
}
 