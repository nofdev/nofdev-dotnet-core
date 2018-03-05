using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Nofdev.Service
{
    public interface IParametersSerializer
    {
        IEnumerable<object> Deserialize(string rawParams, Type[] paramTypes);

        Task<IEnumerable<object>> DeserializeAsync(string rawParams, Type[] paramTypes);
    }

    public class ParametersJsonSerializer : IParametersSerializer
    {
        #region Implementation of IParametersSerializer

        public IEnumerable<object> Deserialize(string rawParams, Type[] paramTypes)
        {
            var array = JArray.Parse(rawParams);
            return array.Select((item, i) => item.ToObject(paramTypes[i]));
        }

        public async Task<IEnumerable<object>> DeserializeAsync(string rawParams, Type[] paramTypes)
        {
            var array = JArray.Parse(rawParams);
            return await Task.Run(() => array.Select((item, i) => item.ToObject(paramTypes[i])));
        }

        #endregion
    }
}
