using System;
using System.Security.Cryptography;
using System.Text;

namespace Wacai.Open.SDK.CSharp
{
    /// <summary>
    ///  消息摘要工具类
    /// </summary>
    public sealed class MDUtil
    {
        /// <summary>
        /// 生成MD5消息摘要
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static String CreateMD5(String data)
        {
            // Use input string to calculate MD5 hash
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(data);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        /// <summary>
        /// 生成消息摘要(基于密码生成摘要）
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="secret">密码</param>
        /// <returns>消息摘要</returns>
        public static string CreateSign(string message, string secret)
        {
            String sign = "";
            if (String.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("Message is null,from Hmac");
            }

            secret = secret ?? "";
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                sign = Convert.ToBase64String(hashmessage);
            }

            sign= sign.Replace("+", "-").Replace("/","_").TrimEnd('=');
            return sign;
        }
    }
}
