using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Java.Util.Concurrent;
using System.IO;
using Square.OkHttp3;
using IMAS.OkHttp.Models;
using Javax.Net.Ssl;
using Java.Security.Cert;
using Java.Security;
using Android.Content;
using Java.Net;
using IMAS.CupCake.Extensions;
using IMAS.CupCake.Data;

namespace IMAS.OkHttp.Bases
{

    public class SSLSocketFactoryEx : SSLSocketFactory
    {

        public SSLSocketFactoryEx(KeyStore truststore, char[] arry)
        {

        }


        public override Socket CreateSocket(InetAddress host, int port)
        {
            throw new NotImplementedException();
        }

        public override Socket CreateSocket(string host, int port)
        {
            throw new NotImplementedException();
        }

        public override Socket CreateSocket(InetAddress address, int port, InetAddress localAddress, int localPort)
        {
            throw new NotImplementedException();
        }

        public override Socket CreateSocket(string host, int port, InetAddress localHost, int localPort)
        {
            throw new NotImplementedException();
        }

        public override Socket CreateSocket(Socket s, string host, int port, bool autoClose)
        {
            throw new NotImplementedException();
        }

        public override string[] GetDefaultCipherSuites()
        {
            throw new NotImplementedException();
        }

        public override string[] GetSupportedCipherSuites()
        {
            throw new NotImplementedException();
        }
    }


    public class MyHostnameVerifier : Java.Lang.Object, IHostnameVerifier
    {
        public bool Verify(string hostname, ISSLSession session)
        {
            return true;
        }
    }

    /// <summary>
    /// 自定义信任
    /// </summary>
    public class MyTrustManager : Java.Lang.Object, IX509TrustManager
    {
        private IX509TrustManager defaultTrustManager;
        private IX509TrustManager localTrustManager;


        public MyTrustManager(IX509TrustManager localTrustManager)
        {
            this.localTrustManager = localTrustManager;
            var factory = TrustManagerFactory.GetInstance(TrustManagerFactory.DefaultAlgorithm);
            factory.Init(default(KeyStore));
            defaultTrustManager = (IX509TrustManager)factory.GetTrustManagers().Where(t => t is IX509TrustManager).FirstOrDefault();
        }


        public void CheckClientTrusted(X509Certificate[] chain, string authType)
        {

        }

        public void CheckServerTrusted(X509Certificate[] chain, string authType)
        {
            try
            {
                defaultTrustManager.CheckServerTrusted(chain, authType);
            }
            catch (Exception)
            {
                localTrustManager.CheckServerTrusted(chain, authType);
            }
        }

        public X509Certificate[] GetAcceptedIssuers()
        {
            return new X509Certificate[0];
        }
    }

    public class UnSafeTrustManager : Java.Lang.Object, IX509TrustManager
    {
        public void CheckClientTrusted(X509Certificate[] chain, string authType)
        {

        }

        public void CheckServerTrusted(X509Certificate[] chain, string authType)
        {

        }

        public X509Certificate[] GetAcceptedIssuers()
        {
            return new X509Certificate[] { };
        }
    }


    /// <summary>
    /// 自定义okhttp请求方式
    /// @：添加和修改使用请求对象 
    /// </summary>
    public abstract class OkHttpBase
    {
        /// <summary>
        /// 获取基础路径
        /// </summary>
        protected string _baseUrl;
        /// <summary>
        /// 请求客户端
        /// </summary>
        private OkHttpClient _client;
        /// <summary>
        /// header
        /// </summary>
        protected Headers _headers;

        /// <summary>
        /// 本地异常
        /// </summary>
        public const int LocalExceptionState = -100000;

        /// <summary>
        /// 证书异常
        /// </summary>
        public const int ACExceptionState = -120000;

        /// <summary>
        /// 网站异常
        /// </summary>
        public const int WebExceptionState = -110000;

        /// <summary>
        /// 上下文
        /// </summary>
        protected Context mContext;
        /// <summary>
        /// 文件路径
        /// </summary>
        private string mFilePath;
        /// <summary>
        /// 是否使用自用证书
        /// </summary>
        protected bool isUseOwnCert = false;

        /// <summary>
        /// key密码
        /// </summary>
        private string mClientKeyPwd;

        public OkHttpBase()
        {

        }


        //protected void InitHeaders(Context context, Dictionary<string, string> dictHeader)
        //{
        //    this.mContext = context;
        //    if (dictHeader != null && dictHeader.Count > 0)
        //    {
        //        var builder = new Headers.Builder();
        //        foreach (var item in dictHeader)
        //        {
        //            builder.Add(item.Key, item.Value.Trim());
        //        }
        //        _headers = builder.Build();
        //    }
        //    else
        //    {
        //        _headers = new Headers.Builder().Build();
        //    }
        //}

        //protected void InitHeaders(Context context, string filePath, string keyPwd, Dictionary<string, string> dictHeader)
        //{
        //    this.mContext = context;
        //    this.mFilePath = filePath;
        //    this.mClientKeyPwd = keyPwd;

        //    if (dictHeader != null && dictHeader.Count > 0)
        //    {
        //        var builder = new Headers.Builder();
        //        foreach (var item in dictHeader)
        //        {
        //            builder.Add(item.Key, item.Value);
        //        }
        //        _headers = builder.Build();
        //    }
        //    else
        //    {
        //        _headers = new Headers.Builder().Build();
        //    }
        //}

        /// <summary>
        /// 会话过期
        /// </summary>
        public const int SessionOverdueState = -120000;

        /// <summary>
        /// 初始化header
        /// </summary>
        /// <param name="dictHeader">header参数集合</param>
        protected void InitHeaders(Dictionary<string, string> dictHeader)
        {
            if (dictHeader != null && dictHeader.Count > 0)
            {
                var builder = new Headers.Builder();
                foreach (var item in dictHeader)
                {
                    builder.Add(item.Key, item.Value.Trim());
                }
                _headers = builder.Build();
            }
            else
            {
                _headers = new Headers.Builder().Build();
            }
        }

        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <returns></returns>
        protected OkHttpClient GetClient()
        {
            if (_client == null)
            {
                var builder = new OkHttpClient.Builder();
                builder.ConnectTimeout(60, TimeUnit.Seconds);

                //if (isUseOwnCert)
                //{
                //    var sslSocketFactory = GetSocketFactoryNext();
                //    if (sslSocketFactory != null)
                //    {
                //        builder.SslSocketFactory(sslSocketFactory, new UnSafeTrustManager());//(sslSocketFactory, new UnSafeTrustManager());
                //    }
                //}

                //builder.HostnameVerifier(new MyHostnameVerifier());

                _client = builder.Build();

            }

            return _client;
        }


        /// <summary>
        /// 添加证书
        /// </summary>
        /// <param name="certificates">证书列表</param>
        /// <returns></returns>
        private SSLSocketFactory GetSocketFactoryNext()
        {
            try
            {
                //创建证书工厂
                CertificateFactory certificateFactory = CertificateFactory.GetInstance("X.509");
                //获取apk数字签名
                KeyStore keyStore = KeyStore.GetInstance(KeyStore.DefaultType);
                keyStore.Load(null, null);

                //var streamL = mContext.Assets.Open("cacert.cer");
                //keyStore.SetCertificateEntry("ygtest", certificateFactory.GenerateCertificate(streamL));
                //trustStore.Load(mContext.Assets.Open("cacert.pfx"), "yg366.com".ToCharArray());
                keyStore.SetCertificateEntry("ygtest", certificateFactory.GenerateCertificate(YgCaCer()));

                //获取套接字的上下文
                var sslContext = SSLContext.GetInstance("TLS");
                TrustManagerFactory trustManagerFactory = TrustManagerFactory.GetInstance(TrustManagerFactory.DefaultAlgorithm);
                trustManagerFactory.Init(keyStore);
                sslContext.Init(null, trustManagerFactory.GetTrustManagers(), new SecureRandom());

                return sslContext.SocketFactory;//new IgnoreSSLSocketFactory(sslContext);//
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        /// <summary>
        /// 添加证书
        /// </summary>
        /// <param name="certificates">证书列表</param>
        /// <returns></returns>
        private SSLSocketFactory GetSocketFactory(List<Stream> certificates)
        {
            if (certificates == null || certificates.Count <= 0)
            {
                return null;
            }

            try
            {
                //创建证书工厂
                CertificateFactory certificateFactory = CertificateFactory.GetInstance("X.509");
                //获取apk数字签名
                KeyStore keyStore = KeyStore.GetInstance(KeyStore.DefaultType);
                keyStore.Load(null);

                for (int i = 0; i < certificates.Count; i++)
                {
                    var certificate = certificates[i];
                    if (certificate != null)
                    {
                        keyStore.SetCertificateEntry("" + i, certificateFactory.GenerateCertificate(certificate));
                        certificate.Close();
                    }
                }

                //获取套接字的上下文
                var sslContext = SSLContext.GetInstance("TLS");
                TrustManagerFactory trustManagerFactory = TrustManagerFactory.GetInstance(TrustManagerFactory.DefaultAlgorithm);
                trustManagerFactory.Init(keyStore);
                sslContext.Init(null, trustManagerFactory.GetTrustManagers(), new SecureRandom());

                return sslContext.SocketFactory;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        private Stream YgCaCer()
        {
            //var key = "-----BEGIN CERTIFICATE-----" + "\n" + "MIID6zCCAtOgAwIBAgIJAP+TSW+yI/PtMA0GCSqGSIb3DQEBCwUAMIGLMQswCQYD";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("-----BEGIN CERTIFICATE-----");
            sb.AppendLine("MIID3zCCAsegAwIBAgIJAIVZGGzYArYEMA0GCSqGSIb3DQEBCwUAMIGFMQswCQYD");
            sb.AppendLine("VQQGEwJDTjEQMA4GA1UECAwHU0lDSFVBTjEQMA4GA1UEBwwHQ0hFTkdEVTELMAkG");
            sb.AppendLine("A1UECgwCWUcxETAPBgNVBAsMCFNPRlRXQVJFMRUwEwYDVQQDDAxjYS55ZzM2Ni5j");
            sb.AppendLine("b20xGzAZBgkqhkiG9w0BCQEWDGNhQHlnMzY2LmNvbTAeFw0xODAxMDUwMjI2NDNa");
            sb.AppendLine("Fw0yODAxMDMwMjI2NDNaMIGFMQswCQYDVQQGEwJDTjEQMA4GA1UECAwHU0lDSFVB");
            sb.AppendLine("TjEQMA4GA1UEBwwHQ0hFTkdEVTELMAkGA1UECgwCWUcxETAPBgNVBAsMCFNPRlRX");
            sb.AppendLine("QVJFMRUwEwYDVQQDDAxjYS55ZzM2Ni5jb20xGzAZBgkqhkiG9w0BCQEWDGNhQHln");
            sb.AppendLine("MzY2LmNvbTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAPr9Zf5jy5OH");
            sb.AppendLine("u6SSC4q5Tql018z7mChzkZUGtZ2I0An/OQjwFBCnvkQIw9r+lArEkBNBlD8IkO29");
            sb.AppendLine("/QehHoTmQ/itsOwUc6mKkQV0MDZYjiRJwtKdBPLIzqJVPukB5JDpE+e44b8ejENi");
            sb.AppendLine("jdbKP+D9FRcm5GZsBqsbEtr/IZ7WY1A/LLOLnj5aPXfvYeemT7f2b78uiEQSOa4u");
            sb.AppendLine("PrpdzQhjAKPMAq+7m7Wpuh9sKrAvpm7KKLxy+plWwQKkuGfjXOkZVvwjepoO5iK2");
            sb.AppendLine("+EmKenJt6eWUc/pu9QoxsCtUrh+piGjYw08KcWw8iqAdzw8M5JjKkwVwCAbtplws");
            sb.AppendLine("q+QwqZJWzZUCAwEAAaNQME4wHQYDVR0OBBYEFJ6RMa73A93B/5K/NmJQZHPKpe6n");
            sb.AppendLine("MB8GA1UdIwQYMBaAFJ6RMa73A93B/5K/NmJQZHPKpe6nMAwGA1UdEwQFMAMBAf8w");
            sb.AppendLine("DQYJKoZIhvcNAQELBQADggEBAM+7zylWA8e5ciV6Q6x8e5z5XNl+p4uG+9sw0JJi");
            sb.AppendLine("70YaVZudoWp/rXAy5AjSuoYJCWQG6Fq1OD3X0lwI4QhLGXZq9Fos1i6Crji13h3f");
            sb.AppendLine("jDzCsfgKR6+t900mXVyEG0lOjeYd0ftVlf5/yrvfV+kWltlU0z+PP8X3VaHJSezP");
            sb.AppendLine("h5MlMSuhVqvOkXP8sGEIWe0MqNOuIDLWDtsD3vVWNlqT9KNzZSky8UyG7cvOzX2j");
            sb.AppendLine("ggKhuiLy/+BtcMB8G5S7Q10JYmEf34Q32xpOQ3XO7PiIrz3FMCJ0dundRe9I5PPW");
            sb.AppendLine("nwNfq/MWvSl47ftH7Yt9hmCwiu5TUjYCMX7rWQyG0eKzIDw=");
            sb.AppendLine("-----END CERTIFICATE-----");

            var ms = new MemoryStream(Encoding.Default.GetBytes(sb.ToString()));
            return ms;
            //new StringReader(sb.ToString());
        }

        /// <summary>
        /// 带有回调函数的方法 异步Post请求
        /// </summary>
        /// <typeparam name="TResult">返回结果</typeparam>
        /// <param name="api">API接口</param>
        /// <param name="paras">请求参数</param>
        /// <param name="onResponse">成功回调函数</param>
        /// <param name="onFailure">失败回调函数</param>
        /// <param name="tag">用于控制取消请求</param>
        /// <returns></returns>
        protected async Task<TResult> AsyncPostEnqueue<TResult>(string api, Dictionary<string, string> paras, Java.Lang.Object tag = null) where TResult : Result, new()
        {
            var builder = new Request.Builder();
            builder.Url($"{_baseUrl}{api}");
            if (tag != null)
            {
                builder.Tag(tag);
            }

            builder.Headers(_headers);

            try
            {
                var feBuilder = new FormBody.Builder();
                if (paras != null || paras.Count > 0)
                {
                    foreach (var item in paras)
                    {
                        feBuilder.Add(item.Key, item.Value);
                    }
                }
                //构建请求参数体
                var rb = feBuilder.Build();
                //构建请求体
                Request request = builder.Post(rb).Build();

                var rst = new TResult();

                //获取请求结果
                //GetClient().NewCall(request).Enqueue(async (response) =>
                //{
                //    //获取请求内容体
                //    var content = await response.Body().StringAsync();
                //    //强转对象
                //    rst = content.ToObject<TResult>();
                //}, (r, ex) =>
                //{
                //    rst = new TResult();
                //    rst.Code = 30;
                //    rst.Message = $"{ex.Message}";
                //});

                GetClient().NewCall(request).Enqueue(new MyOkHttpCallback(async (resp) =>
                {
                    //获取请求内容体
                    var content = await resp.Body().StringAsync();
                    //强转对象
                    rst = content.ToObject<TResult>();
                }, (req, ex) =>
                {
                    rst = new TResult();
                    rst.Code = WebExceptionState;
                    rst.Message = $"{ex.Message}";
                }));

                return rst;
            }
            catch (Exception ex)
            {
                var rst = new TResult();
                rst.Code = LocalExceptionState;
                rst.Message = $"{ex.Message}";
                return rst;
            }
        }

        /// <summary>
        /// 异步Post请求
        /// </summary>
        /// <typeparam name="TResult">返回结果</typeparam>
        /// <param name="api">API接口</param>
        /// <param name="jsonData">请求参数</param>
        /// <param name="tag">用于控制取消请求</param>
        /// <returns></returns>
        protected async Task<TResult> AsyncPostJson<TResult>(string api, string jsonData, Java.Lang.Object tag = null) where TResult : Result, new()
        {
            var builder = new Request.Builder();
            builder.Url($"{_baseUrl}{api}");
            if (tag != null)
            {
                builder.Tag(tag);
            }
            builder.Headers(_headers);

            try
            {
                //构建请求参数体
                var rb = RequestBody.Create(MediaType.Parse(MediaTypeConstants.JSON), string.IsNullOrWhiteSpace(jsonData) ? "{}" : jsonData);
                //构建请求体
                Request request = builder.Post(rb).Build();
                //获取请求结果
                var response = await GetClient().NewCall(request).ExecuteAsync();
                int code = response.Code();
                switch (code)
                {
                    case 401://权限过期
                        var rst = new TResult();
                        rst.Code = SessionOverdueState;
                        rst.Message = $"权限已过期,请重新登录";
                        return rst;
                }

                if (code != 200)
                {
                    var rst = new TResult();
                    rst.Code = code;
                    rst.Message = $"服务器出错";
                    return rst;
                }

                //获取请求内容体
                var content = await response.Body().StringAsync();
                if (!content.StartsWith("{"))
                {
                    var rst = new TResult();
                    rst.Code = WebExceptionState;
                    rst.Message = $"服务器出错";
                    return rst;
                }

                //强转对象
                var obj = content.ToObject<TResult>();
                return obj;
            }
            catch (Exception ex)
            {
                var rst = new TResult();
                rst.Code = LocalExceptionState;
                rst.Message = $"{ex.Message}";
                return rst;
            }
        }

        #region 特殊处理 主要是给除了校餐云管理之外的api调用使用，特别注意

        /// <summary>
        /// 异步Post请求
        /// </summary>
        /// <typeparam name="TResult">返回结果</typeparam>
        /// <param name="api">API接口</param>
        /// <param name="paras">请求参数</param>
        /// <param name="tag">用于控制取消请求</param>
        /// <returns></returns>
        protected async Task<Result<string>> AsyncPostJsonNext(string api, Dictionary<string, object> paras, Java.Lang.Object tag = null) //where TResult : new()
        {
            var rst = new Result<string>();
            var builder = new Request.Builder();
            builder.Url($"{_baseUrl}{api}");
            if (tag != null)
            {
                builder.Tag(tag);
            }
            builder.Headers(_headers);

            try
            {
                //构建请求参数体
                var rb = RequestBody.Create(MediaType.Parse(MediaTypeConstants.JSON), paras == null || paras.Count <= 0 ? "" : paras.ToJson());
                //构建请求体
                Request request = builder.Post(rb).Build();
                //获取请求结果
                var response = await GetClient().NewCall(request).ExecuteAsync();
                //获取请求返回码
                int code = response.Code();
                //switch (code)
                //{
                //    case 401://权限过期
                //        rst.Code = code;
                //        rst.Message = $"权限已过期,请重新登录";
                //        return rst;
                //}

                //if (code != 200)
                //{
                //    //var rst = new Result<TResult>();
                //    rst.Code = WebExceptionState;
                //    rst.Message = $"服务器出错";
                //    return rst;
                //}

                //获取请求内容体
                var content = await response.Body().StringAsync();
                //强转对象
                //var obj = content.ToObject<TResult>();
                //var rst = new Result<TResult>();
                rst.IsSuccess = true;
                rst.Data = content;
                return rst;
            }
            catch (Exception ex)
            {
                //var rst = new TResult();
                rst.Code = LocalExceptionState;
                rst.Message = $"{ex.Message}";
                return rst;
            }
        }


        /// <summary>
        /// 文件上传请求
        /// </summary>
        /// <param name="api">API接口</param>
        /// <param name="filePath">上传文件的绝对路径</param>
        /// <param name="tag"></param>
        /// <returns></returns>
        protected async Task<Result<string>> UploadFilePostNext(string api, string filePath, string mediaTypeStr = "*/*", string nameKey = "upload", Java.Lang.Object tag = null) //where TResult : new()
        {
            var rst = new Result<string>();
            var builder = new Request.Builder();
            builder.Url($"{_baseUrl}{api}");
            if (tag != null)
            {
                builder.Tag(tag);
            }

            if (string.IsNullOrWhiteSpace(mediaTypeStr))
            {
                mediaTypeStr = "*/*";
            }

            builder.Headers(_headers);
            try
            {
                var fileBody = RequestBody.Create(MediaType.Parse(mediaTypeStr), new Java.IO.File(filePath));
                var arrays = filePath.Split('/');
                var fileName = arrays[arrays.Length - 1];

                var requestBody = new MultipartBody.Builder()
                    .SetType(MultipartBody.Form)
                    .AddFormDataPart("image", fileName, fileBody)
                    //.AddFormDataPart("image", "kk.jpg", fileBody)
                    .Build();

                //构建请求体
                Request request = builder.Post(requestBody).Build();
                //获取请求结果
                var response = await GetClient().NewCall(request).ExecuteAsync();
                //获取请求内容体
                var content = await response.Body().StringAsync();
                //if (!content.StartsWith("{"))
                //{
                //    rst.Code = WebExceptionState;
                //    rst.Message = $"服务器出错";
                //    return rst;
                //}
                //强转对象
                //var obj = content.ToObject<TResult>();
                rst.IsSuccess = true;
                rst.Data = content;

                return rst;
            }
            catch (Exception ex)
            {
                rst.Code = LocalExceptionState;
                rst.Message = $"{ex.Message}";
                return rst;
            }
        }

        #endregion 

        /// <summary>
        /// 异步Post请求
        /// </summary>
        /// <typeparam name="TResult">返回结果</typeparam>
        /// <param name="api">API接口</param>
        /// <param name="paras">请求参数</param>
        /// <param name="tag">用于控制取消请求</param>
        /// <returns></returns>
        protected async Task<TResult> AsyncPostJson<TResult>(string api, Dictionary<string, object> paras, Java.Lang.Object tag = null) where TResult : Result, new()
        {
            var builder = new Request.Builder();
            builder.Url($"{_baseUrl}{api}");
            if (tag != null)
            {
                builder.Tag(tag);
            }
            builder.Headers(_headers);
            try
            {
                //构建请求参数体
                var rb = RequestBody.Create(MediaType.Parse(MediaTypeConstants.JSON), paras == null || paras.Count <= 0 ? "" : paras.ToJson());
                //构建请求体
                Request request = builder.Post(rb).Build();
                //获取请求结果
                var response = await GetClient().NewCall(request).ExecuteAsync();
                //获取请求返回码
                int code = response.Code();
                switch (code)
                {
                    case 401://权限过期
                        var rst = new TResult();
                        rst.Code = SessionOverdueState;
                        rst.Message = $"权限已过期,请重新登录";
                        return rst;
                }

                if (code != 200)
                {
                    var rst = new TResult();
                    rst.Code = WebExceptionState;
                    rst.Message = $"服务器出错";
                    return rst;
                }

                //获取请求内容体
                var content = await response.Body().StringAsync();
                //强转对象
                var obj = content.ToObject<TResult>();
                return obj;
            }
            catch (SSLException sslEx)
            {
                var rst = new TResult();
                rst.Code = ACExceptionState;
                rst.Message = $"{sslEx.Message}";
                return rst;
            }
            catch (Exception ex)
            {
                var rst = new TResult();
                rst.Code = LocalExceptionState;
                rst.Message = $"{ex.Message}";
                return rst;
            }
        }

        /// <summary>
        /// 异步Post请求
        /// </summary>
        /// <typeparam name="TResult">返回结果</typeparam>
        /// <param name="api">API接口</param>
        /// <param name="paras">请求参数</param>
        /// <param name="tag">用于控制取消请求</param>
        /// <returns></returns>
        protected async Task<TResult> AsyncPostForm<TResult>(string api, Dictionary<string, string> paras, Java.Lang.Object tag = null) where TResult : Result, new()
        {
            var builder = new Request.Builder();
            builder.Url($"{_baseUrl}{api}");
            if (tag != null)
            {
                builder.Tag(tag);
            }
            builder.Headers(_headers);

            try
            {
                var feBuilder = new FormBody.Builder();
                if (paras != null || paras.Count > 0)
                {
                    foreach (var item in paras)
                    {
                        feBuilder.Add(item.Key, item.Value);
                    }
                }
                //构建请求参数体
                var rb = feBuilder.Build();
                //构建请求体
                Request request = builder.Post(rb).Build();
                //获取请求结果
                var response = await GetClient().NewCall(request).ExecuteAsync();
                //获取请求内容体
                var content = await response.Body().StringAsync();
                if (!content.StartsWith("{"))
                {
                    var rst = new TResult();
                    rst.Code = WebExceptionState;
                    rst.Message = $"服务器出错";
                    return rst;
                }
                //强转对象
                var obj = content.ToObject<TResult>();
                return obj;
            }
            catch (Exception ex)
            {
                var rst = new TResult();
                rst.Code = LocalExceptionState;
                rst.Message = $"{ex.Message}";
                return rst;
            }
        }

        /// <summary>
        /// 异步Get请求
        /// </summary>
        /// <typeparam name="TResult">返回结果</typeparam>
        /// <param name="api">API接口</param>
        /// <param name="tag">用于控制取消请求</param>
        /// <returns></returns>
        protected async Task<TResult> AsyncGet<TResult>(string api, Java.Lang.Object tag = null) where TResult : Result, new()
        {
            var builder = new Request.Builder();
            builder.Get().Url($"{_baseUrl}{api}");
            if (tag != null)
            {
                builder.Tag(tag);
            }
            builder.Headers(_headers);
            try
            {
                //构建请求
                Request request = builder.Build();
                //获取请求结果
                var response = await GetClient().NewCall(request).ExecuteAsync();
                //获取请求内容体
                var content = await response.Body().StringAsync();
                if (!content.StartsWith("{"))
                {
                    var rst = new TResult();
                    rst.Code = WebExceptionState;
                    rst.Message = $"服务器出错";
                    return rst;
                }
                //强转对象
                var obj = content.ToObject<TResult>();
                return obj;
            }
            catch (Exception ex)
            {
                var rst = new TResult();
                rst.Code = LocalExceptionState;
                rst.Message = $"{ex.Message}";
                return rst;
            }
        }

        /// <summary>
        /// 数据实体
        /// </summary>
        /// <returns></returns>
        protected Dictionary<string, object> NewPostParamDataDict()
        {
            var data = new Dictionary<string, object>();
            return data;
        }

        /// <summary>
        /// 取消请求
        /// </summary>
        /// <param name="tag">调用取消请求方法名称</param>
        protected void Cancel(Java.Lang.Object tag)
        {
            var calls = _client.Dispatcher().RunningCalls();
            if (calls != null && calls.Count > 0)
            {
                var call = calls.Where(t => t.Request().Tag() == tag).FirstOrDefault();
                call.Cancel();
            }
        }

        /// <summary>
        /// 取消所有的请求
        /// </summary>
        protected void CancelAll()
        {
            _client.Dispatcher().CancelAll();
        }


        /// <summary>
        /// 文件上传请求
        /// </summary>
        /// <param name="api">API接口</param>
        /// <param name="filePath">上传文件的绝对路径</param>
        /// <param name="tag"></param>
        /// <returns></returns>
        protected async Task<TResult> UploadFilePost<TResult>(string api, string filePath, string mediaTypeStr = "*/*", string nameKey = "upload", Java.Lang.Object tag = null) where TResult : Result, new()
        {
            var builder = new Request.Builder();
            builder.Url($"{_baseUrl}{api}");
            if (tag != null)
            {
                builder.Tag(tag);
            }

            if (string.IsNullOrWhiteSpace(mediaTypeStr))
            {
                mediaTypeStr = "*/*";
            }

            builder.Headers(_headers);
            try
            {
                var fileBody = RequestBody.Create(MediaType.Parse(mediaTypeStr), new Java.IO.File(filePath));
                var arrays = filePath.Split('/');
                var fileName = arrays[arrays.Length - 1];

                var requestBody = new MultipartBody.Builder()
                    .SetType(MultipartBody.Form)
                    .AddFormDataPart("image", fileName, fileBody)
                    //.AddFormDataPart("image", "kk.jpg", fileBody)
                    .Build();

                //构建请求体
                Request request = builder.Post(requestBody).Build();
                //获取请求结果
                var response = await GetClient().NewCall(request).ExecuteAsync();
                //获取请求内容体
                var content = await response.Body().StringAsync();
                if (!content.StartsWith("{"))
                {
                    var rst = new TResult();
                    rst.Code = WebExceptionState;
                    rst.Message = $"服务器出错";
                    return rst;
                }
                //强转对象
                var obj = content.ToObject<TResult>();
                return obj;
            }
            catch (Exception ex)
            {
                var rst = new TResult();
                rst.Code = LocalExceptionState;
                rst.Message = $"{ex.Message}";
                return rst;
            }
        }


        /// <summary>
        /// 批量文件上传请求
        /// </summary>
        /// <param name="api">API接口</param>
        /// <param name="info">上传文件集合</param>
        /// <param name="nameKey">远程服务器接收列表文件的key名称</param>
        /// <param name="mediaTypeStr">文件类型</param>
        /// <param name="tag"></param>
        /// <returns></returns>
        protected async Task<TResult> BatchUploadFilePost<TResult>(string api, IList<UploadFileInfo> info, string nameKey = "upload", string mediaTypeStr = "*/*", Java.Lang.Object tag = null) where TResult : Result, new()
        {
            var builder = new Request.Builder();
            builder.Url($"{_baseUrl}{api}");
            if (tag != null)
            {
                builder.Tag(tag);
            }

            if (string.IsNullOrWhiteSpace(mediaTypeStr))
            {
                mediaTypeStr = "*/*";
            }

            builder.Headers(_headers);
            try
            {
                //var fileBody = RequestBody.Create(MediaType.Parse(mediaTypeStr), new Java.IO.File(filePath));
                //var arrays = filePath.Split('/');
                //var fileName = arrays[arrays.Length - 1];
                var mbbuilder = new MultipartBody.Builder().SetType(MultipartBody.Form);
                foreach (var item in info)
                {
                    item.FileName = item.FilePath.Substring(item.FilePath.LastIndexOf('/') + 1);
                    mbbuilder.AddFormDataPart(nameKey, item.FileName, RequestBody.Create(MediaType.Parse(mediaTypeStr), new Java.IO.File(item.FilePath)));
                }

                var requestBody = mbbuilder.Build();
                //构建请求体
                Request request = builder.Post(requestBody).Build();
                //获取请求结果
                var response = await GetClient().NewCall(request).ExecuteAsync();
                //获取请求内容体
                var content = await response.Body().StringAsync();
                if (!content.StartsWith("{"))
                {
                    var rst = new TResult();
                    rst.Code = WebExceptionState;
                    rst.Message = $"服务器出错";
                    return rst;
                }
                //强转对象
                var obj = content.ToObject<TResult>();
                return obj;
            }
            catch (Exception ex)
            {
                var rst = new TResult();
                rst.Code = LocalExceptionState;
                rst.Message = $"{ex.Message}";
                return rst;
            }
        }

    }
}
