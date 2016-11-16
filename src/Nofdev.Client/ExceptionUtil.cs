using System;
using Nofdev.Core;

namespace Nofdev.Client
{
    public class ExceptionUtil
    {
        //private static readonly ILogger logger = LogHelper.LoggerManager.GetLogger(typeof (ExceptionUtil));

        public static bool IsClassExist(string name)
        {
            var flag = false;
            try
            {
                Type.GetType(name);
                flag = true;
            }
            catch
            {
                //this is just test method
                //no need to process this exception
            }
            return flag;
        }

        public static Exception GetExceptionInstance(string name, string msg)
        {
            try
            {
                var type = Type.GetType(name);
                return (Exception) Activator.CreateInstance(type, msg);
            }
            catch (Exception e)
            {
                //logger.Error(e,() => "Create Exception instance faild!");
                return e;
            }
        }

        public static Exception GetExceptionInstance(ExceptionMessage exceptionMessage)
        {
            try
            {
                var type = Type.GetType(exceptionMessage.Name);
                return (Exception) Activator.CreateInstance(type, exceptionMessage.Msg);
            }
            catch (Exception e)
            {
                //logger.Error(e,()=> "Create Exception instance faild!");
                return e;
            }
        }
    }
}