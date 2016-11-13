using System;
using System.Collections.Generic;

namespace Nofdev.Server
{
    public  class ServiceRegistry : Dictionary<string,Type>
    {
        protected ServiceRegistry()
        {
            
        }
        public static ServiceRegistry Instance { get; private set; }

        static ServiceRegistry()
        {
            Instance = new ServiceRegistry();
        }

        public Type Get(string key)
        {
            return ContainsKey(key) ? this[key] : null;
        }

        public void AddOrUpdate(Dictionary<string, Type> dict)
        {
            foreach (var item in dict)
            {
                if (ContainsKey(item.Key))
                    this[item.Key] = item.Value;
                else
                {
                    Add(item.Key,item.Value);
                }
            }
        }
    }
}
