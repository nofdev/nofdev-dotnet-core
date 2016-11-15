using System;

namespace Nofdev.Core.Repository
{
    public class SoftDeleteAttribute : Attribute
    {
        public SoftDeleteAttribute(int deleteStatus = -1)
        {
            DeleteStatus = deleteStatus;
        }

        public int DeleteStatus { get; }
    }
}
