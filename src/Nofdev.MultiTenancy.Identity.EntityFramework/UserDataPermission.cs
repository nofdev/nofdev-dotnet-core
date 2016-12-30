using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class UserDataPermission<TKey, TUserKey> : ImmutableModel<TKey, TUserKey>
    {
        public TUserKey UserId { get; set; }

        /// <summary>
        ///     key
        /// </summary>
        public string DataKey { get; set; }

        /// <summary>
        /// data items,split by '|'
        /// </summary>
        public string DataItems { get; set; }

    }
}
