using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Wacai.Open.SDK.CSharp.libs
{
    /// <summary>
    /// Http Client参数类
    /// </summary>
    public class HttpClientParam
    {
        private const String _DEFAULT_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";

        public HttpClientParam() { }

        public String url { set; get; }
        public String contentType { get; set; }
        public IDictionary<string, string> headers { get; set; }
        public IDictionary<string, string> bodyParams { get; set; }
        public CookieCollection cookies { set; get; }
        public byte[] request { set; get; }
        public bool isMultiPart { get; set; }
        private int timeout = 1000;
        public int ConnectTimeout
        {
            get
            {
                return this.timeout;
            }
            set
            {
                if (value > 0)
                {
                    this.timeout = value;
                }
            }
        }

        private Encoding requestEncoding = Encoding.UTF8;
        public Encoding RequestEncoding
        {
            get
            {
                return this.requestEncoding;
            }
            set
            {
                this.requestEncoding = value;
            }
        }


        private String userAgent = _DEFAULT_AGENT;
        public String UserAgent
        {
            get
            {
                return this.userAgent;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    this.userAgent = value;
                }
            }
        }
    }
}
