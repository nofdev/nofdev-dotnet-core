using System.Collections.Generic;
using System.Threading;

namespace Nofdev.Core.SOA
{
    /// <summary>
    /// context of service passed by HTTP Header
    /// </summary>
    public class ServiceContext
    {
        /// <summary>
        /// dictionary key of service context
        /// </summary>
        public const string ServiceContextKey = "Nofdev.Core.ServiceContext-KEY";

        /// <summary>
        /// key of HTTP header
        /// </summary>
        public const string HttpHeaderKey = "SERVICE-CONTEXT";

        private readonly Dictionary<string, object> _items = new Dictionary<string, object>();
        private const string CallIdKey = "CALLID";
        private const string TenantContextKey = "TENANT";
       private static readonly ReaderWriterLockSlim _locker = new ReaderWriterLockSlim();
        private ServiceContext()
        {
            CallId = new CallId();
        }

        /// <summary>
        /// Call ID
        /// </summary>
        public CallId CallId
        {
            get
            {
                return this[CallIdKey] as CallId;
            }
            set
            {
                this[CallIdKey] = value;
            }
        }

        /// <summary>
        /// User
        /// </summary>
        public User User
        {
            get
            {
                return this[TenantContextKey] as User;
            }
            set
            {
                this[TenantContextKey] = value;
            }
        }

        /// <summary>
        /// Get items in a dictionary
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, object> GetItems()
        {
            return _items;
        }

        /// <summary>
        /// index
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get {
                key = key.ToUpper();
                return _items[key]; }
            set {
                key = key.ToUpper();
                if (_items.ContainsKey(key))
                    _items[key] = value;
                else
                    _items.Add(key, value);
            }
        }

        /// <summary>
        /// singleton
        /// </summary>
        public static ServiceContext Current
        {
            get
            {
                _locker.EnterReadLock();
                var context = NeutralContext.Current.Get(ServiceContextKey) as ServiceContext;
                _locker.ExitReadLock();
                if (context == null)
                {
                
                    _locker.EnterWriteLock();
                    context = new ServiceContext();
                    NeutralContext.Current.Set(ServiceContextKey, context);
                    _locker.ExitWriteLock();
                }
                return context;
            }
        }

        /// <summary>
        /// refresh call ID
        /// </summary>
        public void RefreshCallId()
        {
            CallId = (CallId == null ? new CallId() : CallId.NewSub());
        }
    }

    public enum ServiceType
    {
        Facade,
        Service,
        Micro
    }
}