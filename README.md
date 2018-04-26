# wacai-open-sdk-csharp

## API网关
### 接口协议
- 使用HTTP协议作为目前的交互协议
- 客户端统一使用POST方式向开放平台API网关提交数据
- 请求报文、响应报文格式都是JSON，content_type为application/json
- 交互的编码格式统一为UTF-8
- HTTP正常响应的http code都是200，非正常返回400

### 使用配置
- 申请app_key/app_secret，使用前，先向开放平台申请;
- app_key/app_secret替换,替换为步骤1申请的(在app.config中修改)
- 修改地址(生产环境),系统上线时，需要修改网关地址和token获取地址(在app.php中修改)

### 使用实例-获取token
```java
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
```

### 使用实例-调用API接口
```java
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
```

### 注意事项
调用API接口需要鉴权，这个过程中会使用到Token, 根据app_key/app_secret获取访问access_token, 
接下来访问时，会自动带上access_token, 如果access_token 过期或失效，会再次请求进行token获取置换，在此过程中，需要注意两点：
- 为了提高性能，获取到token尽量放在分布式缓存(memcache/redis)中，考虑到各个接入方技术栈不同，难以强制统一，本demo中access_token放在服务器内存中；
- 如果access_token 过期或失效，SDK会自动进行token获取置换，但不会自动发起请求重试操作，请求重试，由调用方发起；