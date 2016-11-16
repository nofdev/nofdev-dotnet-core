using System;
using Microsoft.AspNetCore.Hosting;

namespace Nofdev.Sample
{
    public class Startup : Nofdev.Server.Startup
    {
        public Startup(IHostingEnvironment env) : base(env)
        {
           
        }
    }

    public interface IHelloFacade
    {
        DateTime GetNow();
    }

    public class HelloFacade : IHelloFacade
    {
        #region Implementation of IHelloFacade

        public DateTime GetNow()
        {
            return DateTime.Now;
        }

        #endregion
    }
}
