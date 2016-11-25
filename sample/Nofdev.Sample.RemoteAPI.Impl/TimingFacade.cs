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

        #endregion
    }
}