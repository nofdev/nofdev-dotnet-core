using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class OrganizationUnit<T,TUser> : MutableModel<T,TUser>
    {
        public T ParentId { get; set; }
    }
}
