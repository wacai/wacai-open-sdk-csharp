using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wacai.open.sdk.csharp;
using wacai.open.sdk.csharp.token;

namespace open_sdk_csharp_app
{
    class Programasaaaa
    {
        static void Mainaaaa1(string[] args)
        {
            for (int i = 0; i < 1; i++)
            {
                TokenInfo token = TokenService.Get();
                if (token != null)
                {
                    token = TokenService.Refresh(token.refreshToken);
                    Console.WriteLine(token.accessToken);
                }
            }
            /*
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            Console.WriteLine((long)timeSpan.TotalMilliseconds);
             Console.WriteLine(timestamp);
            */

            Console.WriteLine("Hello World!");
            Console.Read();
        }
    }
}
