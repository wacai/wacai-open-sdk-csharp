using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Wacai.Open.SDK.CSharp.config;
using Wacai.Open.SDK.CSharp.libs;

namespace Wacai.Open.SDK.CSharp.token
{
    /// <summary>
    /// Token 业务逻辑类(含获取和刷新token)
    /// </summary>
    public class TokenService
    {    
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns>token 类</returns>
        public static TokenInfo Get()
        {
            String timestamp = Utils.GetTimeStamp().ToString();
            // Request header
            IDictionary<string, string> bodyParams = new Dictionary<String, String>();
            bodyParams.Add("app_key", Configer.APP_KEY);
            bodyParams.Add("grant_type", Constants._CLIENT_CREDENTIALS);
            bodyParams.Add("timestamp", timestamp);

            return Fetch(bodyParams, Configer.APP_SECRET, Constants._FETCH_TOKEN_PATH);
        }

        /// <summary>
        /// 刷新token(通过refresh token获取新token)
        /// </summary>
        /// <param name="refreshToken">RereshToken</param>
        /// <returns>Token class</returns>
        public static TokenInfo Refresh(String refreshToken)
        {
            String timestamp = Utils.GetTimeStamp().ToString();
            // Request header
            IDictionary<string, string> bodyParams = new Dictionary<String, String>();
            bodyParams.Add("app_key", Configer.APP_KEY);
            bodyParams.Add("grant_type", "refresh_token");
            bodyParams.Add("refresh_token", refreshToken);
            bodyParams.Add("timestamp", timestamp);

            return Fetch(bodyParams, Configer.APP_SECRET, Constants._REFRESH_TOKEN_PATH);
        }

        /// <summary>
        /// 检查token是否过期
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public static bool CheckExpire(byte[] res)
        {
            bool isExpire = false;
            String resStr = Encoding.UTF8.GetString(res);
            dynamic response = JObject.Parse(resStr);
            String error = response.code;
            /*
            INVALID_REFRESH_TOKEN(10010, "非法的refresh_token"),
            ACCESS_TOKEN_EXPIRED(10011, "access_token已过期"),
            INVALID_ACCESS_TOKEN(10012, "access_token非法"),
            REFRESH_TOKEN_EXPIRED(10013, "refresh_token已过期"),;
            */
            if ((!String.IsNullOrEmpty(error)) 
                && (error.Equals("10011") || error.Equals("10013")))
            {
                isExpire = true;
            }     
            return isExpire;
        }

        /// <summary>
        /// 获取的token(封装了通过http访问token server)
        /// </summary>
        /// <param name="bodyParams"></param>
        /// <param name="appSecret"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private static TokenInfo Fetch(IDictionary<string, string> bodyParams, String appSecret,String path)
        {
            // 待签名
            StringBuilder sbToSign = new StringBuilder();
            foreach (var item in bodyParams)
            {
                sbToSign.Append(item.Value);
            }
            String toSign = sbToSign.ToString();
            // 已签名
            String signature = MDUtil.CreateSign(toSign, appSecret);
            bodyParams.Add("sign", signature);
            // 获取token的url
            String url = Configer.GW_TOKEN_URL + path;

            byte[] response = HttpClient.HttpPost(url, null, bodyParams, Encoding.UTF8);
            if (response != null && response.Length > 0)
            {
                String responseData = (Encoding.UTF8).GetString(response, 0, response.Length);
                return convert(responseData);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static TokenInfo convert(String data)
        {
            TokenInfo tokenInfo = new TokenInfo();
            dynamic token = JObject.Parse(data);
            tokenInfo.accessToken = token.access_token;
            tokenInfo.refreshToken = token.refresh_token;
            tokenInfo.expiresIn = token.expires_in;
            return tokenInfo;
        }
    }
}
