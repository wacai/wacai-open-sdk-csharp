using System;

namespace Wacai.Open.SDK.CSharp.token
{
    /// <summary>
    /// Token信息类 
    /// </summary>
    public class TokenInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TokenInfo() { }

        public String accessToken { get; set; }
        public String refreshToken { get; set; }
        public String expiresIn { get; set; }
    }
}
