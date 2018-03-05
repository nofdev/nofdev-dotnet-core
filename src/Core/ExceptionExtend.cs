using System;

namespace Nofdev.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExceptionExtend
    {
       // public bool EnableStackTrace { get; set; } = false;
       /// <summary>
       /// 
       /// </summary>
       /// <param name="exception"></param>
       /// <returns></returns>
        public static ExceptionMessage FormatException(this Exception exception)
        {
            if (exception == null) return null;
            var exceptionMessage = new ExceptionMessage();
            exceptionMessage.Name = exception.GetType().Name;
            exceptionMessage.Msg = exception.Message;
           // exceptionMessage.cause = exception.InnerException;
         //   if (EnableStackTrace)
         //   {
                exceptionMessage.Stack = exception.StackTrace;
          //  }
            return exceptionMessage;
        }
    }
}
