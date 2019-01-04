using Android.Support.V4.App;
using Java.IO;
using Java.Nio;
using Java.Nio.Channels;
using Square.OkHttp3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using IMAS.OkHttp.Bases;

namespace IMAS.OkHttp.Download
{
    /// <summary>
    /// 自定义进度条下载器
    /// </summary>
    public class ProgressDownloader
    {
        private IProgressListener progressListener;
        private Java.IO.File destination;
        public string Url { get; private set; }
        private OkHttpClient client;
        private ICall call;
        public NotificationCompat.Builder Builder { get; private set; }
        public int Id { get; private set; }
        public string Tag { get; private set; }

        /// <summary>
        /// 总大小
        /// </summary>
        public long TotalLen { get; set; }

        /// <summary>
        /// 设置下载大小
        /// </summary>
        public long DTotalBytes { get; set; }

        /// <summary>
        /// 保存控制器
        /// </summary>
        public int SaveFlag { get; set; }

        public long StartPoint { get; private set; }

        public ProgressDownloader(string url, string tag, string destinationPath, IProgressListener progressListener)
        {
            this.Url = url;
            this.Tag = tag;
            this.destination = new Java.IO.File(destinationPath);
            this.progressListener = progressListener;
            //在下载、暂停后的继续下载中可复用同一个client对象
            client = GetProgressClient();
        }

        public ProgressDownloader(int id, NotificationCompat.Builder builder, string url, string destinationPath, IProgressListener progressListener)
        {
            this.Id = id;
            this.Builder = builder;
            this.Url = url;
            this.destination = new Java.IO.File(destinationPath);
            this.progressListener = progressListener;
            //在下载、暂停后的继续下载中可复用同一个client对象
            client = GetProgressClient();
        }

        /// <summary>
        /// 获取client对象
        /// </summary>
        /// <returns></returns>
        private OkHttpClient GetProgressClient()
        {
            return new OkHttpClient.Builder().AddNetworkInterceptor(new MYProgressInterceptor(this, progressListener)).Build();
        }

        /// <summary>
        /// 每次下载需要新建新的Call对象
        /// </summary>
        /// <param name="startPoints">开始的点</param>
        /// <returns></returns>
        private ICall NewCall(long startPoints)
        {
            Request request = new Request.Builder()
                .Url(Url)
                .Header("RANGE", "bytes=" + startPoints + "-")//断点续传要用到的，指示下载的区间
                .Build();
            return client.NewCall(request);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="startPoints">指定开始下载的点</param>
        public void Download(long startPoints)
        {
            this.StartPoint = startPoints;
            call = NewCall(startPoints);
            call.Enqueue(new MyOkHttpCallback((response) =>
            {
                Save(response, startPoints);
            }, (req, ex) =>
            {

            }));

        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="response"></param>
        /// <param name="startsPoint"></param>
        private void Save(Response response, long startsPoint)
        {
            Stream stream = null;
            RandomAccessFile randomAccessFile = null;
            FileChannel channelOut = null;
            try
            {
                ResponseBody body = response.Body();
                stream = body.ByteStream();
                randomAccessFile = new RandomAccessFile(destination, "rwd");
                //Chanel NIO中的用法，由于RandomAccessFile没有使用缓存策略，直接使用会使得下载速度变慢，亲测缓存下载3.3秒的文件，用普通的RandomAccessFile需要20多秒。
                channelOut = randomAccessFile.Channel;
                // 内存映射，直接使用RandomAccessFile，是用其seek方法指定下载的起始位置，使用缓存下载，在这里指定下载位置。
                GC.Collect();
                MappedByteBuffer mappedBuffer = channelOut.Map(FileChannel.MapMode.ReadWrite, startsPoint, body.ContentLength());
                byte[] buffer = new byte[1024];
                int len = -1;
                while ((len = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    mappedBuffer.Put(buffer, 0, len);
                }

            }
            catch (Java.Lang.Exception ex)
            {
                try
                {
                    if (stream != null)
                    {
                        stream.Close();
                        stream.Dispose();
                    }

                    if (channelOut != null)
                    {
                        channelOut.Close();
                        channelOut.Dispose();
                    }

                    if (randomAccessFile != null)
                    {
                        randomAccessFile.Close();
                        randomAccessFile.Dispose();
                    }
                }
                catch (Java.IO.IOException ioex)
                {

                }
            }

        }

        /// <summary>
        /// 暂停下载
        /// </summary>
        public void Pause()
        {
            if (call != null)
            {
                call.Cancel();
            }
        }

    }

    /// <summary>
    /// 自定义进度条拦截器
    /// </summary>
    public class MYProgressInterceptor : Java.Lang.Object, IInterceptor
    {
        private IProgressListener progressListener;
        private ProgressDownloader sender;

        public MYProgressInterceptor(ProgressDownloader sender, IProgressListener progressListener)
        {
            this.sender = sender;
            this.progressListener = progressListener;
        }

        public Response Intercept(IInterceptorChain chain)
        {
            Response originalResponse = chain.Proceed(chain.Request());
            return originalResponse.NewBuilder().Body(new ProgressResponseBody(originalResponse.Body(), sender, progressListener)).Build();
        }
    }


}
