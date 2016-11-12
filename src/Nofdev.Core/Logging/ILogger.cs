using System;

namespace Nofdev.Core.Logging
{
    /// <summary>
    /// The ILogger interface is use by the framework to log messages.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// debug
        /// </summary>
        /// <param name="obj"></param>
        void Debug(object obj);

        /// <summary>
        /// debug and output exception
        /// </summary>
        /// <param name="e"></param>
        /// <param name="obj"></param>
        void Debug(Exception e, object obj);

        /// <summary>
        /// error and output exception
        /// </summary>
        /// <param name="obj"></param>
        void Error(object obj);

        /// <summary>
        /// error 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="obj"></param>
        void Error(Exception e, object obj);

        /// <summary>
        /// fatal
        /// </summary>
        /// <param name="obj"></param>
        void Fatal(object obj);

        /// <summary>
        /// fatal and output exception
        /// </summary>
        /// <param name="e"></param>
        /// <param name="obj"></param>
        void Fatal(Exception e, object obj);

        /// <summary>
        /// info
        /// </summary>
        /// <param name="obj"></param>
        void Info(object obj);

        /// <summary>
        /// info and output exception
        /// </summary>
        /// <param name="e"></param>
        /// <param name="obj"></param>
        void Info(Exception e, object obj);


        /// <summary>
        /// warn
        /// </summary>
        /// <param name="obj"></param>
        void Warn(object obj);

        /// <summary>
        /// warn and output exception
        /// </summary>
        /// <param name="e"></param>
        /// <param name="obj"></param>
        void Warn(Exception e, object obj);

        /// <summary>
        /// debug
        /// </summary>
        /// <param name="callback"></param>
        void Debug(Func<object> callback);

        /// <summary>
        /// debug and output exception
        /// </summary>
        /// <param name="e"></param>
        /// <param name="callback"></param>
        void Debug(Exception e, Func<object> callback);

        /// <summary>
        /// error and output exception
        /// </summary>
        /// <param name="callback"></param>
        void Error(Func<object> callback);

        /// <summary>
        /// error 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="callback"></param>
        void Error(Exception e,Func<object> callback);

        /// <summary>
        /// fatal
        /// </summary>
        /// <param name="callback"></param>
        void Fatal(Func<object> callback);

        /// <summary>
        /// fatal and output exception
        /// </summary>
        /// <param name="e"></param>
        /// <param name="callback"></param>
        void Fatal(Exception e, Func<object> callback);

        /// <summary>
        /// info
        /// </summary>
        /// <param name="callback"></param>
        void Info(Func<object> callback);

        /// <summary>
        /// info and output exception
        /// </summary>
        /// <param name="e"></param>
        /// <param name="callback"></param>
        void Info(Exception e, Func<object> callback);


        /// <summary>
        /// warn
        /// </summary>
        /// <param name="callback"></param>
        void Warn(Func<object> callback);

        /// <summary>
        /// warn and output exception
        /// </summary>
        /// <param name="e"></param>
        /// <param name="callback"></param>
        void Warn(Exception e, Func<object> callback);
    }

    /// <summary>
    ///  This ILoggerManager interface is used by client applications to request logger instances.
    /// </summary>
    public interface ILoggerManager
    {
        /// <summary>
        /// get logger by  type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        ILogger GetLogger(Type type);

        /// <summary>
        /// get logger by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ILogger GetLogger(string key);

        /// <summary>
        /// get logger by generic type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ILogger GetLogger<T>();
    }
}