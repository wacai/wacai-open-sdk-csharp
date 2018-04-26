using System;
using System.Text;
using Wacai.Open.SDK.CSharp.api;

namespace Open.SDK.CSharp.App
{
    public class ApiPostDemo
    {
        public static void Main(String[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                // 调用的api name
                String apiName = "api.test.post.fixed";
                // 调用的api 版本
                String apiVersion = "1.0";
                // api client
                ApiClient apiClient = new ApiClient(apiName, apiVersion);
                // 请求的body-json报文
                String body = "{\"uid\":123,\"name\":\"zy\"}";
                // 发送请求
                byte[] res = apiClient.PostJSON(body);
                // 解析响应
                String resStr = Encoding.UTF8.GetString(res);
                Console.WriteLine(String.Format("Response={0}", resStr));
            }
            Console.Read();
        }
    }
}
