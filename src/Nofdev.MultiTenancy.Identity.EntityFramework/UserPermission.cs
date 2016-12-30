﻿using System;
using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class UserPermission<TKey, TUserKey, TPermission> : IMutableModel<TKey, TUserKey>
    {
        /// <summary>
        ///     permission code
        /// </summary>
        public string Code { get; set; }

        public TUserKey UserId { get; set; }

        public TPermission PermissionId { get; set; }

        public bool IsGranted { get; set; }
        public virtual TKey Id { get; set; }

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