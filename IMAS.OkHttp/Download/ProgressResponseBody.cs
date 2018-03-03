using Square.OkHttp3;
using Square.OkIO;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMAS.OkHttp.Download
{
    /// <summary>
    /// 进度监听接口
    /// </summary>
    public interface IProgressListener
    {
        /// <summary>
        /// 文件总长只需记录一次，要注意断点续传后的contentLength只是剩余部分的长度
        /// </summary>
        /// <param name="contentLength"></param>
        void OnPreExecute(ProgressDownloader sender, long contentLength);

        /// <summary>
        /// 更新进度
        /// </summary>
        /// <param name="totalBytes">总长度</param>
        /// <param name="done">是否下载完成</param>
        void Update(ProgressDownloader sender, long totalBytes, bool done);

        /// <summary>
        /// 更新错误
        /// </summary>
        /// <param name="errorMsg">错误信息</param>
        void UpdateError(ProgressDownloader sender, string errorMsg);
    }


    /// <summary>
    /// 带进度返回内容体
    /// </summary>
    public class ProgressResponseBody : ResponseBody
    {
        private ResponseBody responseBody;
        private IBufferedSource bufferedSource;
        private IProgressListener progressListener;
        private ProgressDownloader sender;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="responseBody">响应体</param>
        /// <param name="listener">监听</param>
        public ProgressResponseBody(ResponseBody responseBody, ProgressDownloader sender, IProgressListener listener)
        {
            this.sender = sender;
            this.responseBody = responseBody;
            this.progressListener = listener;
            if (progressListener != null)
            {
                progressListener.OnPreExecute(sender, ContentLength());
            }
        }

        public override long ContentLength()
        {
            return responseBody.ContentLength();
        }

        public override MediaType ContentType()
        {
            return responseBody.ContentType();
        }

        public override IBufferedSource Source()
        {
            if (bufferedSource == null)
            {
                bufferedSource = OkIO.Buffer(Source(responseBody.Source()));
            }

            return bufferedSource;
        }

        private ISource Source(ISource source)
        {
            return new MYForwardingSource(sender, progressListener, source);
        }

    }

    /// <summary>
    /// 自定义资源转发器
    /// </summary>
    public class MYForwardingSource : ForwardingSource
    {
        private IProgressListener progressListener;
        private long totalBytes = 0L;
        private ProgressDownloader sender;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="delegate"></param>
        public MYForwardingSource(ProgressDownloader sender, IProgressListener listener, ISource @delegate) : base(@delegate)
        {
            this.sender = sender;
            this.progressListener = listener;
        }

        public override long Read(OkBuffer sink, long byteCount)
        {
            try
            {
                var bytesRead = base.Read(sink, byteCount);
                totalBytes += bytesRead != -1 ? bytesRead : 0;
                if (progressListener != null)
                {
                    progressListener.Update(sender, totalBytes, bytesRead == -1);
                }

                return bytesRead;
            }
            catch (Exception ex)
            {
                if (progressListener != null)
                    progressListener.UpdateError(sender, $"{ex.Message}");
                return 0L;
            }
        }

    }


}
