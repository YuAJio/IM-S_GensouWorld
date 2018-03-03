using System;
using System.Collections.Generic;
using System.Text;

namespace IMAS.OkHttp.Models
{
    /// <summary>
    /// 文件下载状态
    /// </summary>
    public enum DownLoadState
    {
        /// <summary>
        /// 占位符
        /// </summary>
        None,
        /// <summary>
        /// 准备
        /// </summary>
        Prepare,
        /// <summary>
        /// 开始
        /// </summary>
        Start,
        /// <summary>
        /// 暂停
        /// </summary>
        Pause,
        /// <summary>
        /// 下载停止
        /// </summary>
        Stop,
        /// <summary>
        /// 下载中
        /// </summary>
        Downloading,
        /// <summary>
        /// 下载完成
        /// </summary>
        Finished,
        /// <summary>
        /// 下载中异常
        /// </summary>
        Exception,
    }


    /// <summary>
    /// 上传文件
    /// </summary>
    public class UploadFileInfo
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }
    }

    /// <summary>
    /// 文件信息实体类
    /// </summary>
    public class DownLoadFileInfo
    {
        /// <summary>
        /// 地址
        /// @网络请求地址
        /// @唯一标识文件
        /// </summary>
        [SQLite.PrimaryKey]
        public string FileUrl { get; set; }

        /// <summary>
        /// 下载状态
        /// </summary>
        public DownLoadState DownLoadState { get; set; }

        /// <summary>
        /// 本地地址
        /// </summary>
        public string LocalPath { get; set; }
        /// <summary>
        /// 临时本地地址
        /// </summary>
        public string TempLocalPath { get; set; }

        /// <summary>
        /// 开始大小
        /// </summary>
        public long Start { get; set; }

        /// <summary>
        /// 现在下载大小
        /// </summary>
        public long Now { get; set; }

        /// <summary>
        /// 文件总大小
        /// </summary>
        public long Length { get; set; }

    }

}
