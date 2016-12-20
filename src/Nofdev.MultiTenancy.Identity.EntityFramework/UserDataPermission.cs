using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class UserDataPermission<TKey, TUser> : IMutableModel<TKey, TUser>
    {
        public TKey Id { get; set; }


        public TUser UserId { get; set; }

        /// <summary>
        ///     key
        /// </summary>
        public string DataKey { get; set; }

        /// <summary>
        /// data items,split by '|'
        /// </summary>
        public string DataItems { get; set; }


        #region Implementation of IImmutableModel<TKey,TUser>

        public DateTime CreatedDate { get; set; }
        public TUser CreatedBy { get; set; }

        #endregion

        #region Implementation of IMutableModel<TKey,TUser>

        public TUser UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        #endregion
    }
}
