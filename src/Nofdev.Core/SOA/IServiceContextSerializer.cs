using System.Collections.Specialized;

namespace Nofdev.Core.SOA
{
    public interface IServiceContextSerializer
    {
        NameValueCollection ToNameValueCollection(ServiceContext context);

        ServiceContext Deserialize(NameValueCollection collection);
    }

 
}