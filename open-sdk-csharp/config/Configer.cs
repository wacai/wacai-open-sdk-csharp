using System;
using System.Configuration;

namespace Wacai.Open.SDK.CSharp.config
{
    /// <summary>
    /// 配置类
    /// </summary>
    public sealed class Configer
    {
        /// <summary>
        /// API网关地址
        /// </summary>
        public static string GW_URL
        {
            get
            {
                 return ConfigurationManager.AppSettings["GW_URL"];
            }
        }

        /// <summary>
        /// API_网关_Token获取地址
        /// </summary>
        public static string GW_TOKEN_URL
        {
            get
            {
                return ConfigurationManager.AppSettings["GW_TOKEN_URL"];
            }
        }

        /// <summary>
        /// 消息网关地址
        /// </summary>
        public static string GW_MESSAGE_URL
        {
            get
            {
                return ConfigurationManager.AppSettings["GW_MESSAGE_URL"];
            }
        }
        /// <summary>
        /// 消息网关端口
        /// </summary>
        public static string GW_MESSAGE_URL_PORT
        {
            get
            {
                return ConfigurationManager.AppSettings["GW_MESSAGE_URL_PORT"];
            }
        }

        /// <summary>
        /// APP_KEY
        /// </summary>
        public static string APP_KEY
        {
            get
            {
                return ConfigurationManager.AppSettings["APP_KEY"];
            }
        }

        /// <summary>
        /// APP_SECRET
        /// </summary>
        public static string APP_SECRET
        {
            get
            {
                return ConfigurationManager.AppSettings["APP_SECRET"];
            }
        }

        /// <summary>
        /// WAC_Version
        /// </summary>
        public static string WAC_VERSION
        {
            get
            {
                return ConfigurationManager.AppSettings["X_WAC_VERSION"];
            }
        }
    }
}
