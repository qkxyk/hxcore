using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using log4net.Repository;

namespace HXCloud.Common
{
    public sealed class Logger
    {
        #region 饿汉模式
        /*
        private static readonly Logger _logger = new Logger();
        private static readonly log4net.ILog _Logger4net = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>  
        /// 无参私有构造函数  
        /// </summary>  
        private Logger()
        {
        }
        /// 得到单例  
        /// </summary>  
        public static Logger Singleton
        {
            get
            {
                return _logger;
            }
        }
        */
        #endregion

        #region [ 单例模式 ]
        private static Logger _logger;
        private static log4net.ILog _Logger4net;// = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static ILoggerRepository repository;
        private static readonly object SyncObject = new object();

        /// <summary>  
        /// 无参私有构造函数  
        /// </summary>  
        private Logger()
        {
            if (_logger == null)
            {
                repository = LogManager.CreateRepository("hxlog");
                //log4net从log4net.config文件中读取配置信息
                XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
                //log = LogManager.GetLogger(repository.Name, "InfoLogger");
            }
        }

        /// <summary>  
        /// 得到单例  
        /// </summary>  
        public static Logger Singleton
        {
            get
            {
                if (_logger == null)
                {
                    lock (SyncObject)
                    {
                        if (_logger == null)
                        {
                            _logger = new Logger();
                        }
                    }
                }
                return _logger;
            }
        }
        #endregion

        #region [ 参数 ]
        public static void SetLog(string loginfo)
        {
            _Logger4net = LogManager.GetLogger(repository.Name, loginfo);
        }
        public bool IsDebugEnabled
        {
            get { return _Logger4net.IsDebugEnabled; }
        }
        public bool IsInfoEnabled
        {
            get { return _Logger4net.IsInfoEnabled; }
        }
        public bool IsWarnEnabled
        {
            get { return _Logger4net.IsWarnEnabled; }
        }
        public bool IsErrorEnabled
        {
            get { return _Logger4net.IsErrorEnabled; }
        }
        public bool IsFatalEnabled
        {
            get { return _Logger4net.IsFatalEnabled; }
        }
        #endregion  

        #region [ 接口方法 ]

        #region [ Debug ]
        public void Debug(string message)
        {
            if (this.IsDebugEnabled)
            {
                this.Log(LoggerLevel.Debug, message);
            }
        }

        public void Debug(string message, Exception exception)
        {
            if (this.IsDebugEnabled)
            {
                this.Log(LoggerLevel.Debug, message, exception);
            }
        }

        public void DebugFormat(string format, params object[] args)
        {
            if (this.IsDebugEnabled)
            {
                this.Log(LoggerLevel.Debug, format, args);
            }
        }

        public void DebugFormat(string format, Exception exception, params object[] args)
        {
            if (this.IsDebugEnabled)
            {
                this.Log(LoggerLevel.Debug, string.Format(format, args), exception);
            }
        }
        #endregion

        #region [ Info ]
        public void Info(string message)
        {
            if (this.IsInfoEnabled)
            {
                this.Log(LoggerLevel.Info, message);
            }
        }

        public void Info(string message, Exception exception)
        {
            if (this.IsInfoEnabled)
            {
                this.Log(LoggerLevel.Info, message, exception);
            }
        }

        public void InfoFormat(string format, params object[] args)
        {
            if (this.IsInfoEnabled)
            {
                this.Log(LoggerLevel.Info, format, args);
            }
        }

        public void InfoFormat(string format, Exception exception, params object[] args)
        {
            if (this.IsInfoEnabled)
            {
                this.Log(LoggerLevel.Info, string.Format(format, args), exception);
            }
        }
        #endregion

        #region  [ Warn ]

        public void Warn(string message)
        {
            if (this.IsWarnEnabled)
            {
                this.Log(LoggerLevel.Warn, message);
            }
        }

        public void Warn(string message, Exception exception)
        {
            if (this.IsWarnEnabled)
            {
                this.Log(LoggerLevel.Warn, message, exception);
            }
        }

        public void WarnFormat(string format, params object[] args)
        {
            if (this.IsWarnEnabled)
            {
                this.Log(LoggerLevel.Warn, format, args);
            }
        }

        public void WarnFormat(string format, Exception exception, params object[] args)
        {
            if (this.IsWarnEnabled)
            {
                this.Log(LoggerLevel.Warn, string.Format(format, args), exception);
            }
        }
        #endregion

        #region  [ Error ]

        public void Error(string message)
        {
            if (this.IsErrorEnabled)
            {
                this.Log(LoggerLevel.Error, message);
            }
        }

        public void Error(string message, Exception exception)
        {
            if (this.IsErrorEnabled)
            {
                this.Log(LoggerLevel.Error, message, exception);
            }
        }

        public void ErrorFormat(string format, params object[] args)
        {
            if (this.IsErrorEnabled)
            {
                this.Log(LoggerLevel.Error, format, args);
            }
        }

        public void ErrorFormat(string format, Exception exception, params object[] args)
        {
            if (this.IsErrorEnabled)
            {
                this.Log(LoggerLevel.Error, string.Format(format, args), exception);
            }
        }
        #endregion

        #region  [ Fatal ]

        public void Fatal(string message)
        {
            if (this.IsFatalEnabled)
            {
                this.Log(LoggerLevel.Fatal, message);
            }
        }

        public void Fatal(string message, Exception exception)
        {
            if (this.IsFatalEnabled)
            {
                this.Log(LoggerLevel.Fatal, message, exception);
            }
        }

        public void FatalFormat(string format, params object[] args)
        {
            if (this.IsFatalEnabled)
            {
                this.Log(LoggerLevel.Fatal, format, args);
            }
        }

        public void FatalFormat(string format, Exception exception, params object[] args)
        {
            if (this.IsFatalEnabled)
            {
                this.Log(LoggerLevel.Fatal, string.Format(format, args), exception);
            }
        }
        #endregion
        #endregion

        #region [ 内部方法 ]  
        /// <summary>  
        /// 输出普通日志  
        /// </summary>  
        /// <param name="level"></param>  
        /// <param name="format"></param>  
        /// <param name="args"></param>  
        private void Log(LoggerLevel level, string format, params object[] args)
        {
            switch (level)
            {
                case LoggerLevel.Debug:
                    _Logger4net.DebugFormat(format, args);
                    break;
                case LoggerLevel.Info:
                    _Logger4net.InfoFormat(format, args);
                    break;
                case LoggerLevel.Warn:
                    _Logger4net.WarnFormat(format, args);
                    break;
                case LoggerLevel.Error:
                    _Logger4net.ErrorFormat(format, args);
                    break;
                case LoggerLevel.Fatal:
                    _Logger4net.FatalFormat(format, args);
                    break;
            }
        }

        /// <summary>  
        /// 格式化输出异常信息  
        /// </summary>  
        /// <param name="level"></param>  
        /// <param name="message"></param>  
        /// <param name="exception"></param>  
        private void Log(LoggerLevel level, string message, Exception exception)
        {
            switch (level)
            {
                case LoggerLevel.Debug:
                    _Logger4net.Debug(message, exception);
                    break;
                case LoggerLevel.Info:
                    _Logger4net.Info(message, exception);
                    break;
                case LoggerLevel.Warn:
                    _Logger4net.Warn(message, exception);
                    break;
                case LoggerLevel.Error:
                    _Logger4net.Error(message, exception);
                    break;
                case LoggerLevel.Fatal:
                    _Logger4net.Fatal(message, exception);
                    break;
            }
        }
        #endregion
    }//end of class  


    #region [ enum: LogLevel ]
    /// <summary>  
    /// 日志级别  
    /// </summary>  
    public enum LoggerLevel
    {
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }
    #endregion    
}
