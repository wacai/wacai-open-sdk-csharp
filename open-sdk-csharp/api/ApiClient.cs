using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Wacai.Open.SDK.CSharp.config;
using Wacai.Open.SDK.CSharp.libs;
using Wacai.Open.SDK.CSharp.token;

namespace Wacai.Open.SDK.CSharp.api
{
    /// <summary>
    /// Api Client
    /// </summary>
    public class ApiClient
    {
        #region Constructor

        /// <summary>
        /// 构造函数
        /// </summary>
        public ApiClient() { }
        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="apiName"></param>
        /// <param name="apiVersion"></param>
        public ApiClient(String apiName,String apiVersion)
        {
            this.apiName = apiName;
            this.apiVersion = apiVersion;
        }
        #endregion
       
        #region Fields/Properties

        public String apiName { get; set; }
        public String apiVersion { get; set; }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        private const string _CACHE_TOKEN_PREFIX = "Token_";

        /// <summary>
        /// 发送http-post/json的请求
        /// </summary>
        /// <param name="json">业务数据json格式</param>
        /// <returns>响应response</returns>
        public byte[] PostJSON(String json)
        {
            // Timestamp
            String timestamp = Utils.GetTimeStamp().ToString();

            // 获取token
            TokenInfo tokenInfo = getToken();
            // 组装request-header
            Dictionary<String, String> dicHeader = new Dictionary<String, String>();
            dicHeader.Add("x-wac-version",Configer.WAC_VERSION);
            dicHeader.Add("x-wac-timestamp", timestamp);
            dicHeader.Add("x-wac-access-token", tokenInfo.accessToken);
            // Request-header-order排序
            var orderResult = dicHeader.OrderBy(x => x.Key);
            Dictionary<String, String> orderedHeader = orderResult.ToDictionary(t => t.Key, t => t.Value);

            // 组装待签名
            StringBuilder sbHeaderString = new StringBuilder();
            foreach(var item in orderedHeader)
            {
                sbHeaderString.AppendFormat("{0}={1}&", item.Key, item.Value);
            }
            String headerString = sbHeaderString.ToString().TrimEnd('&');

            // 待签名
            // MD5
            String md5 = MDUtil.CreateMD5(json);
            String toSign = String.Format("{0}|{1}|{2}|{3}", this.apiName, this.apiVersion, headerString, md5);
            // 签名
            String sinature = MDUtil.CreateSign(toSign, Configer.APP_SECRET);

            dicHeader = new Dictionary<String, String>();
            dicHeader.Add("x-wac-version", Configer.WAC_VERSION);
            dicHeader.Add("x-wac-timestamp", timestamp);
            dicHeader.Add("x-wac-access-token", tokenInfo.accessToken);
            dicHeader.Add("x-wac-signature", sinature);

            // 发送请求
            String apiUrl = String.Format("{0}/{1}/{2}", Configer.GW_URL, this.apiName, this.apiVersion);
            byte[] response = HttpClient.HttpPostJson(apiUrl, dicHeader, json, Encoding.UTF8, null);
            
            // 异步检查token(主要考虑到token过期概率低)
            new Thread(new ParameterizedThreadStart(handleToken)).Start(response);
           
            return response;
        }

        /// <summary>
        /// 处理token(检查是否token过期,如果过期的话则再次获取)
        /// </summary>
        /// <param name="obj"></param>
        private void handleToken(Object obj)
        {
            byte[] res = (byte[])obj;
            bool isExpire = TokenService.CheckExpire(res);
            if (!isExpire)
            {
                // 再次获取token
                getToken(!isExpire);
            }
        }

        private TokenInfo getToken(bool isExpire = false)
        {
            String key = _CACHE_TOKEN_PREFIX + Configer.APP_KEY;
            TokenInfo token = null;
            // 如果token失效，则强制再次获取
            if (isExpire)
            {
                token = TokenService.Get();
            }else
            {
                try
                {
                    token = TokenLocalCache.dicToken[key];
                }
                catch(KeyNotFoundException ex)
                {
                    Console.WriteLine(String.Format("Key:{0} not found,error={1}", key, ex.Message));
                }

                // 如果cache token丢失，则再次获取
                if (token == null || String.IsNullOrEmpty(token.accessToken))
                {
                    token = TokenService.Get();
                    TokenLocalCache.dicToken[key] = token;
                }
            }   
  
            return token; 
        }
    }
}
