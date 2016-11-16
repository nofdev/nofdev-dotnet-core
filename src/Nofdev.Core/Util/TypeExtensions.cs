using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nofdev.Core.Util
{
    public static class TypeExtensions
    {
        public static string RemovePrefixI(this string interfaceName)
        {
            if (interfaceName.Length > 1 && interfaceName[0] == 'I' && char.IsUpper(interfaceName[1]))
                return interfaceName.Substring(1);
            return interfaceName;
        }
    }
}
