using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class OrganizationUnit<T,TUserKey> : MutableModel<T,TUserKey>
    {
        public T ParentId { get; set; }
    }
}
