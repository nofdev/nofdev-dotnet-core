using System;
using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class UserPermission<TKey, TUser, TPermission> : IMutableModel<TKey, TUser>
    {
        /// <summary>
        ///     permission code
        /// </summary>
        public string Code { get; set; }

        public TUser UserId { get; set; }

        public TPermission PermissionId { get; set; }

        public bool IsGranted { get; set; }
        public TKey Id { get; set; }

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