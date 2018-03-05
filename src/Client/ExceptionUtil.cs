using System;
using Nofdev.Core;

namespace Nofdev.Client
{
    public class ExceptionUtil
    {
        public static Exception GetExceptionInstance(string name, string msg)
        {
            var type = Type.GetType(name);
            return (Exception) Activator.CreateInstance(type, msg);
        }

        public static Exception GetExceptionInstance(ExceptionMessage exceptionMessage)
        {
            var type = Type.GetType(exceptionMessage.Name);
            return (Exception) Activator.CreateInstance(type, exceptionMessage.Msg);
        }
    }
}