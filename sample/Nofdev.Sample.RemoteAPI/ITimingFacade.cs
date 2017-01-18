using System;

namespace Nofdev.Sample.RemoteAPI
{
    /// <summary>
    /// mockup a remote facade API to test client invoke
    /// </summary>
    public interface ITimingFacade
    {
        DateTime GetNow();
        string Hello(string name);
    }
}
