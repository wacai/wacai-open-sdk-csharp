using System;
using System.Collections.Generic;

namespace Wacai.Open.SDK.CSharp.token
{
    /// <summary>
    /// Token本地缓存(建议:业务方使用分布式缓存memcahe/redis)
    /// </summary>
    public sealed class TokenLocalCache
    {
        /// <summary>
        /// 本地缓存
        /// </summary>
        public static readonly IDictionary<String, TokenInfo> dicToken = new Dictionary<String, TokenInfo>();
    }
}
