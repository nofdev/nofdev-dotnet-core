namespace Nofdev.Core.SOA.Annotations
{
    /// <summary>
    /// Attribute
    /// </summary>
    public class MicroServiceAttribute : ServiceDefinationAttribute
    {
        static MicroServiceAttribute()
        {
            UrlSegment = "micro";
        }
    }
}