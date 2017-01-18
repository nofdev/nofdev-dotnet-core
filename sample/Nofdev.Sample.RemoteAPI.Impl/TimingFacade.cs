using System;

namespace Nofdev.Sample.RemoteAPI.Impl
{
    public class TimingFacade : ITimingFacade
    {
        #region Implementation of ITimingFacade

        public DateTime GetNow()
        {
            return DateTime.Now;
        }

        public string Hello(string name)
        {
            return "hello," + name;
        }

        #endregion
    }
}