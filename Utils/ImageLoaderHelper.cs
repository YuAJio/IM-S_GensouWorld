
using Android.Content;
using Com.Nostra13.Universalimageloader.Core;
using Com.Nostra13.Universalimageloader.Core.Download;
using Java.Net;
using Java.Security;
using Java.Security.Cert;
using Javax.Net.Ssl;
using System.IO;
using System.Text;

namespace IdoMaster_GensouWorld.Utils
{
    #region Https证书相关
    public class AuthImageDownloader : BaseImageDownloader
    {
        private SSLSocketFactory mSSLSocketFactory;
        public AuthImageDownloader(Context context) : base(context)
        {
            mSSLSocketFactory = GetSocketFactoryNext();
        }
        public AuthImageDownloader(Context context, int connectTimeout, int readTimeout) : base(context, connectTimeout, readTimeout)
        {
            mSSLSocketFactory = GetSocketFactoryNext();
        }

        protected override Stream GetStreamFromNetwork(string imageUri, Java.Lang.Object p1)
        {
            URL url = null;
            try
            {
                url = new URL(imageUri);
            }
            catch (MalformedURLException e)
            {
            }
            var conn = url.OpenConnection();
            conn.ConnectTimeout = ConnectTimeout;
            conn.ReadTimeout = ReadTimeout;
            if (conn is HttpsURLConnection)
            {
                //  ((HttpsURLConnection)conn).SSLSocketFactory = mSSLSocketFactory; 若是自定义证书 则需解锁
                ((HttpsURLConnection)conn).HostnameVerifier = new DO_NOT_VERIFY();
            }

            return conn.InputStream;

        }

        private class DO_NOT_VERIFY : Java.Lang.Object, IHostnameVerifier
        {
            public bool Verify(string hostname, ISSLSession session)
            {
                return true;
            }
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
                keyStore.SetCertificateEntry("证书名", certificateFactory.GenerateCertificate(YgCaCer()));

                //获取套接字的上下文
                var sslContext = SSLContext.GetInstance("TLS");
                TrustManagerFactory trustManagerFactory = TrustManagerFactory.GetInstance(TrustManagerFactory.DefaultAlgorithm);
                trustManagerFactory.Init(keyStore);
                sslContext.Init(null, trustManagerFactory.GetTrustManagers(), new SecureRandom());

                return sslContext.SocketFactory;//new IgnoreSSLSocketFactory(sslContext);//
            }
            catch (System.Exception ex)
            {

            }

            return null;
        }

        private Stream YgCaCer()
        {
            //var key = "-----BEGIN CERTIFICATE-----" + "\n" + "MIID6zCCAtOgAwIBAgIJAP+TSW+yI/PtMA0GCSqGSIb3DQEBCwUAMIGLMQswCQYD";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
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

    }
    #endregion
    public class ImageLoaderHelper
    {

        /// <summary>
        /// 通用图片加载Option
        /// </summary>
        /// <returns></returns>
        public static DisplayImageOptions GeneralImageOption()
        {
            DisplayImageOptions options = new DisplayImageOptions.Builder()
                //设置图片在下载期期间显示的图片
                .ShowImageOnLoading(Resource.Mipmap.nameless_all_die)
                //设置图片Uri为空或是显示错误的时候显示的图片
                .ShowImageForEmptyUri(Resource.Mipmap.nameless_all_die)
                //设置图片加载/解码过程中错错误时候显示的图片
                .ShowImageOnFail(Resource.Mipmap.nameless_all_die)
                //设置下载的图片是否缓存在内存中
                .CacheInMemory(true)
                .CacheOnDisc(true)
                //是否考虑JPEG图像EXIF参数(旋转,翻转)
                .ConsiderExifParams(true)
                //55*55
                //设置图片以如何的编码方式显示
                .ImageScaleType(Com.Nostra13.Universalimageloader.Core.Assist.ImageScaleType.ExactlyStretched)
                //设置图片的解码类型
                .BitmapConfig(Android.Graphics.Bitmap.Config.Rgb565)
                //.Displayer(new Com.Nostra13.Universalimageloader.Core.Display.FadeInBitmapDisplayer(100))//是否图片加载后渐入的动画时间
                .Build();
            return options;
        }

        /// <summary>
        /// 背景图片加载设置
        /// </summary>
        /// <returns></returns>
        public static DisplayImageOptions BackGroundImageOption()
        {
            DisplayImageOptions options = new DisplayImageOptions.Builder()
                //设置图片在下载期期间显示的图片
                .ShowImageOnLoading(Resource.Mipmap.defaut_bg)
                //设置图片Uri为空或是显示错误的时候显示的图片
                .ShowImageForEmptyUri(Resource.Mipmap.defaut_bg)
                //设置图片加载/解码过程中错错误时候显示的图片
                .ShowImageOnFail(Resource.Mipmap.defaut_bg)
                //设置下载的图片是否缓存在内存中
                .CacheInMemory(true)
                .CacheOnDisc(true)
                //是否考虑JPEG图像EXIF参数(旋转,翻转)
                .ConsiderExifParams(true)
                //55*55
                //设置图片以如何的编码方式显示
                .ImageScaleType(Com.Nostra13.Universalimageloader.Core.Assist.ImageScaleType.ExactlyStretched)
                //设置图片的解码类型
                .BitmapConfig(Android.Graphics.Bitmap.Config.Rgb565)
                //.Displayer(new Com.Nostra13.Universalimageloader.Core.Display.FadeInBitmapDisplayer(100))//是否图片加载后渐入的动画时间
                .Build();
            return options;
        }

        /// <summary>
        /// 背景图片加载设置
        /// </summary>
        /// <returns></returns>
        public static DisplayImageOptions CharacterPicImageOption()
        {
            DisplayImageOptions options = new DisplayImageOptions.Builder()
                 //设置图片在下载期期间显示的图片
                 .ShowImageOnLoading(Resource.Mipmap.nameless_all_die)
                 //设置图片Uri为空或是显示错误的时候显示的图片
                 .ShowImageForEmptyUri(Resource.Mipmap.nameless_all_die)
                 //设置图片加载/解码过程中错错误时候显示的图片
                 .ShowImageOnFail(Resource.Mipmap.nameless_all_die)
                 //设置下载的图片是否缓存在内存中
                 .CacheInMemory(true)
                 .CacheOnDisc(true)
                 //是否考虑JPEG图像EXIF参数(旋转,翻转)
                 .ConsiderExifParams(true)
                 //55*55
                 //设置图片以如何的编码方式显示
                 .ImageScaleType(Com.Nostra13.Universalimageloader.Core.Assist.ImageScaleType.ExactlyStretched)
                 //设置图片的解码类型
                 .BitmapConfig(Android.Graphics.Bitmap.Config.Rgb565)
                 //.Displayer(new Com.Nostra13.Universalimageloader.Core.Display.FadeInBitmapDisplayer(100))//是否图片加载后渐入的动画时间
                 .Build();
            return options;
        }

        /// <summary>
        /// 圆形图片加载Options
        /// </summary>
        /// <returns></returns>
        public static DisplayImageOptions CircleImageOptions()
        {
            DisplayImageOptions options = new DisplayImageOptions.Builder()
                //设置图片在下载期期间显示的图片
                .ShowImageOnLoading(Resource.Mipmap.icon_defaut_avator)
                //设置图片Uri为空或是显示错误的时候显示的图片
                .ShowImageForEmptyUri(Resource.Mipmap.icon_defaut_avator)
                //设置图片加载/解码过程中错错误时候显示的图片
                .ShowImageOnFail(Resource.Mipmap.icon_defaut_avator)
                //设置下载的图片是否缓存在内存中
                .CacheInMemory(true)
                .CacheOnDisc(true)
                //是否考虑JPEG图像EXIF参数(旋转,翻转)
                .ConsiderExifParams(true)
                //是否设置为圆角,弧度为多少
                .ImageScaleType(Com.Nostra13.Universalimageloader.Core.Assist.ImageScaleType.ExactlyStretched)
                //设置图片的解码类型
                .BitmapConfig(Android.Graphics.Bitmap.Config.Rgb565)
                //是否设置为圆角,弧度为多少
                .Displayer(new Com.Nostra13.Universalimageloader.Core.Display.RoundedBitmapDisplayer(360))
                .Build();
            return options;
        }
    }
}