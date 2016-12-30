using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class UserDataPermission<TKey, TUserKey> : IMutableModel<TKey, TUserKey>
    {
        public virtual TKey Id { get; set; }


        public TUserKey UserId { get; set; }

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
        public TUserKey CreatedBy { get; set; }

        #endregion

        #region Implementation of IMutableModel<TKey,TUser>

        public TUserKey UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        #endregion
    }
}
