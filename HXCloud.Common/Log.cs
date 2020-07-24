using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using log4net;
using log4net.Config;
using log4net.Repository;

namespace HXCloud.Common
{
    //静态类日志
    public static class Log
    {
        private static ILog log;
        private static ILoggerRepository repository;
        static Log()
        {
            if (log == null)
            {
                repository = LogManager.CreateRepository("netcorelog");
                //log4net从log4net.config文件中读取配置信息
                XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
                //log = LogManager.GetLogger(repository.Name, "InfoLogger");
            }
        }
        public static void SetLog(Type t)
        {
            log = LogManager.GetLogger(t);
        }

        public static void SetLog(string loginfo)
        {
            log = LogManager.GetLogger(repository.Name, loginfo);
        }

        public static void Debug(object message)
        {
            log.Debug(message);
        }

        public static void DebugFormatted(string format, params object[] args)
        {
            log.DebugFormat(format, args);
        }

        public static void Info(object message)
        {
            log.Info(message);
        }

        public static void InfoFormatted(string format, params object[] args)
        {
            log.InfoFormat(format, args);
        }

        public static void Warn(object message)
        {
            log.Warn(message);
        }

        public static void Warn(object message, Exception exception)
        {
            log.Warn(message, exception);
        }

        public static void WarnFormatted(string format, params object[] args)
        {
            log.WarnFormat(format, args);
        }

        public static void Error(object message)
        {
            log.Error(message);
        }

        public static void Error(object message, Exception exception)
        {
            log.Error(message, exception);
        }

        public static void ErrorFormatted(string format, params object[] args)
        {
            log.ErrorFormat(format, args);
        }

        public static void Fatal(object message)
        {
            log.Fatal(message);
        }

        public static void Fatal(object message, Exception exception)
        {
            log.Fatal(message, exception);
        }

        public static void FatalFormatted(string format, params object[] args)
        {
            log.FatalFormat(format, args);
        }
    }
}
