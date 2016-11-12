using System.Collections.Concurrent;
using System.Threading;

namespace Nofdev.Core
{

    /// <summary>
    /// NeutralContext
    /// </summary>
    public class NeutralContext : ConcurrentDictionary<string, object>
    {
        /// <summary>
        /// key of neutral context
        /// </summary>
        public const string NeutralContextKey = "Nofdev.Core.NeutralContext-Key";
        static readonly AsyncLocal<NeutralContext> AsyncLocalContexts = new AsyncLocal<NeutralContext>();

        private NeutralContext()
        {

        }

        /// <summary>
        /// singleton
        /// </summary>
        public static NeutralContext Current
        {
            get
            {
                var context =  AsyncLocalContexts.Value ?? (AsyncLocalContexts.Value = new NeutralContext());
                return context;
            }
        }

        /// <summary>
        /// get a value from context by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key)
        {
           
            if (ContainsKey(key))
            {
                return this[key];
            }
            return null;
        }

        /// <summary>
        /// set a key-value pair to context
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, object value)
        {
            this[key] = value;
        }
    }
}
