using System;
using System.Collections.Concurrent;

namespace Nofdev.Core.Message
{
    public class TopicMessage<T>
    {
        public ConcurrentDictionary<string, object> Headers { get; set; } = new ConcurrentDictionary<string, object>();

        public T Payload { get; set; }

        public void AddHeader(String key, String value)
        {
            Headers.AddOrUpdate(key, value, (k, v) => value);
        }
    }
}
