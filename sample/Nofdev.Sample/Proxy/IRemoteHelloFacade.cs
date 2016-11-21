using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nofdev.Sample.Proxy
{
    /// <summary>
    /// mockup a remote facade API to test client invoke
    /// </summary>
    public interface IRemoteHelloFacade
    {
        DateTime GetNow();
    }
}
