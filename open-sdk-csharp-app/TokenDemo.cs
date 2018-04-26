using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wacai.Open.SDK.CSharp.token;

namespace Open.SDK.CSharp.App
{
    public class TokenDemo
    {
        public static void Main1(String[] args)
        {
            // 获取token
            TokenInfo token = TokenService.Get();
            if(token != null)
            {
                Console.WriteLine(String.Format("Acquire token successful, Current access token:{0}", token.accessToken));
                
                // 刷新token(通过refreshToken重新获取token)
                String refreshToken = token.refreshToken;
                TokenInfo tokenNew = TokenService.Refresh(refreshToken);
                if (tokenNew != null)
                {
                    Console.WriteLine(String.Format("Refresh token successful, Current access token:{0}", token.accessToken));
                }
            }

            Console.Read();
        }
    }
}
