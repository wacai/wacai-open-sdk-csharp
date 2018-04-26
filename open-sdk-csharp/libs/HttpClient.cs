using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Text;

namespace Wacai.Open.SDK.CSharp.libs
{
    /// <summary>
    /// HttpClient封装工具类
    /// </summary>
    public sealed class HttpClient
    {
        private const string _BOUNDARY = "----------------------------8223117e5cec";

        private const int _TIMEOUT = 1000;

        public static byte[] HttpPost(String url
            , IDictionary<String,String> headers
            , IDictionary<String,String> bodyParams
            , Encoding requestEncoding)
        {
            HttpClientParam param = new HttpClientParam();
            param.headers = headers;
            param.isMultiPart = false;
            param.contentType = "application/x-www-form-urlencoded";
            if(bodyParams != null && bodyParams.Count > 0)
            {
                StringBuilder buffer = new StringBuilder();
                int index = 0;
                foreach(var item in bodyParams)
                {
                    if(index > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", item.Key, item.Value);
                    }else
                    {
                        buffer.AppendFormat("{0}={1}", item.Key, item.Value);
                    }
                    index++;
                }

                byte[] requestData = requestEncoding.GetBytes(buffer.ToString());
                param.request = requestData;
            }

            return HttpExec(url, param,false);
        }

        public static byte[] HttpPostJson(String url
            , IDictionary<String,String> headers
            , String json
            , Encoding requestEncoding
            , CookieCollection cookies)
        {
            if (requestEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding is null");
            }

            if (String.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException("bizJson is null");
            }

            HttpClientParam param = new HttpClientParam();
            param.contentType="application/json";
            byte[] data = requestEncoding.GetBytes(json);
            param.request = data;
            param.headers = headers;
            param.cookies = cookies;

            return HttpExec(url, param);
        }

        public static byte[] HttpExec(String url
            ,HttpClientParam httpClientParam,bool isMultiPart = false)
        {
            byte[] response = null;
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url is null");
            }

            String contentType = httpClientParam.contentType;
            if (String.IsNullOrWhiteSpace(contentType))
            {
                throw new ArgumentNullException("ContentType is null");
            } 
        
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback +=
    new RemoteCertificateValidationCallback((sender, certificate, chain, policyErrors) => { return true; });
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version11;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }

            request.Method = "POST";
            request.ContentType = contentType;//"application/x-www-form-urlencoded";
            request.Timeout = httpClientParam.ConnectTimeout;

            CookieCollection cookies = httpClientParam.cookies;
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }

            // 添加header
            IDictionary<String, String> headers = httpClientParam.headers;
            if (!(headers == null || headers.Count == 0))
            {
                foreach (var item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }

            // 添加业务参数
            byte[] requestData = httpClientParam.request;
            if (requestData != null && requestData.Length > 0)
            {
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(requestData, 0, requestData.Length);
                }
            }

            if (httpClientParam.isMultiPart)
            {
                request.Headers.Add("Content-Type: multipart/form-data; boundary="+ _BOUNDARY);
                request.Headers.Add("Content-Length: " + httpClientParam.request.Length);
                request.Headers.Add("Expect: ");
            }

            HttpWebResponse httpResponse = null;
            try
            {
                 httpResponse = request.GetResponse() as HttpWebResponse;
            }catch(Exception ex)
            {
                Console.Write(ex);
            }


            if(httpResponse != null)
            { 
                using (Stream stream = httpResponse.GetResponseStream())
                {
                    byte[] bytes = stream.ReadAllBytes();
                    return bytes;
                }
            }

            return response;
        }
    }
}
