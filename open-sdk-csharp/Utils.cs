using System;

namespace Wacai.Open.SDK.CSharp
{
    /// <summary>
    /// 工具类
    /// </summary>
    public sealed class Utils
    {
        /// <summary>
        /// 生成时间戳(长度:13位)
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalMilliseconds;
        }
    }
}
