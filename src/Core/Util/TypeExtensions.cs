using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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

        public static Type GetRealReturnType(this MethodInfo method)
        {
            var returnType = method.ReturnType;
            var returnTypeInfo = returnType.GetTypeInfo();
            var realType = returnType;
            if (returnTypeInfo.IsGenericType && returnTypeInfo.BaseType == typeof(Task))
            {
                realType = returnType.GenericTypeArguments[0];
            }
            else if (returnType.Name == typeof(IEnumerable<>).Name)
            {
                realType = typeof(List<>).MakeGenericType(returnType.GenericTypeArguments[0]);
            }
            else if (returnType == typeof(void))
            {
                realType = typeof(object);
            }
            return realType;
        }
    }
}
