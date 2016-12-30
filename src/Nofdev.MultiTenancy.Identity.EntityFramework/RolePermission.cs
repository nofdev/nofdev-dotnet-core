using System;
using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class RolePermission<TKey, TRoleKey, TPermission, TUserKey> : MutableModel<TKey, TUserKey>
    {
        /// <summary>
        ///     permission code
        /// </summary>
        public string Code { get; set; }

        public TRoleKey RoleId { get; set; }

        public TPermission PermissionId { get; set; }

        public bool IsGranted { get; set; }

    }
}