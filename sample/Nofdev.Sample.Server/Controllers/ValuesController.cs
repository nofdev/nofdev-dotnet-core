using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Nofdev.Sample.RemoteAPI;

namespace Nofdev.Sample.Server.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ITimingFacade _timingFacade;

        public ValuesController(ITimingFacade timingFacade)
        {
            _timingFacade = timingFacade;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("[action]")]
        public DateTime GetNow()
        {
            return _timingFacade.GetNow();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
