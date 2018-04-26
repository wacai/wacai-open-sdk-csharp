# wacai-open-sdk-csharp

## API����
### �ӿ�Э��
- ʹ��HTTPЭ����ΪĿǰ�Ľ���Э��
- �ͻ���ͳһʹ��POST��ʽ�򿪷�ƽ̨API�����ύ����
- �����ġ���Ӧ���ĸ�ʽ����JSON��content_typeΪapplication/json
- �����ı����ʽͳһΪUTF-8
- HTTP������Ӧ��http code����200������������400

### ʹ������
- ����app_key/app_secret��ʹ��ǰ�����򿪷�ƽ̨����;
- app_key/app_secret�滻,�滻Ϊ����1�����(��app.config���޸�)
- �޸ĵ�ַ(��������),ϵͳ����ʱ����Ҫ�޸����ص�ַ��token��ȡ��ַ(��app.php���޸�)

### ʹ��ʵ��-��ȡtoken
```java
// ��ȡtoken
TokenInfo token = TokenService.Get();
if(token != null)
{
    Console.WriteLine(String.Format("Acquire token successful, Current access token:{0}", token.accessToken));
                
    // ˢ��token(ͨ��refreshToken���»�ȡtoken)
    String refreshToken = token.refreshToken;
    TokenInfo tokenNew = TokenService.Refresh(refreshToken);
    if (tokenNew != null)
    {
        Console.WriteLine(String.Format("Refresh token successful, Current access token:{0}", token.accessToken));
    }
}
```

### ʹ��ʵ��-����API�ӿ�
```java
// ���õ�api name
String apiName = "api.test.post.fixed";
// ���õ�api �汾
String apiVersion = "1.0";
// api client
ApiClient apiClient = new ApiClient(apiName, apiVersion);
// �����body-json����
String body = "{\"uid\":123,\"name\":\"zy\"}";
// ��������
byte[] res = apiClient.PostJSON(body);
// ������Ӧ
String resStr = Encoding.UTF8.GetString(res);
Console.WriteLine(String.Format("Response={0}", resStr));
```

### ע������
����API�ӿ���Ҫ��Ȩ����������л�ʹ�õ�Token, ����app_key/app_secret��ȡ����access_token, 
����������ʱ�����Զ�����access_token, ���access_token ���ڻ�ʧЧ�����ٴ��������token��ȡ�û����ڴ˹����У���Ҫע�����㣺
- Ϊ��������ܣ���ȡ��token�������ڷֲ�ʽ����(memcache/redis)�У����ǵ��������뷽����ջ��ͬ������ǿ��ͳһ����demo��access_token���ڷ������ڴ��У�
- ���access_token ���ڻ�ʧЧ��SDK���Զ�����token��ȡ�û����������Զ������������Բ������������ԣ��ɵ��÷�����