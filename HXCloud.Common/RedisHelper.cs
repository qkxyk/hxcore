using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Common
{
    public class RedisHelper
    {
        #region [ 单例模式 ]
        //ConnectionMultiplexer redis =
        private static ConnectionMultiplexer redis;
        public static string redisconf = "192.168.100.7:6379,password=xzc,DefaultDatabase=0";
        private static readonly object SyncObject = new object();

        /// <summary>  
        /// 无参私有构造函数  
        /// </summary>  
        private RedisHelper()
        {
        }

        /// <summary>  
        /// 得到单例  
        /// </summary>  
        public static ConnectionMultiplexer Singleton
        {
            get
            {
                if (redis == null)
                {
                    lock (SyncObject)
                    {
                        if (redis == null)
                        {
                            redis = ConnectionMultiplexer.Connect(redisconf);
                        }
                    }
                }
                return redis;
            }
        }
        #endregion  

    }
}
