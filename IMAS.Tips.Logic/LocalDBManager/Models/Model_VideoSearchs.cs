using System;
using System.Collections.Generic;
using System.Text;

namespace IMAS.LocalDBManager.Models
{
    public class Model_VideoCover
    {
        /// <summary>
        /// 电影名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 缩微图
        /// </summary>
        public string Img { get; set; }

        /// <summary>
        /// 跳转地址
        /// </summary>
        public string Href { get; set; }
    }
    /// <summary>
    /// 影片详细信息
    /// </summary>
    public class VideoMsg : Model_VideoCover
    {
        /// <summary>
        /// 主演
        /// </summary>
        public string Starring { get; set; }

        /// <summary>
        /// 影片类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Add { get; set; }

        /// <summary>
        /// 语言
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 年代
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Intro { get; set; }

        /// <summary>
        /// 播放列表，名称/地址
        /// </summary>
        public List<VideoResources> PlayList { get; set; }
    }
    public class VideoResources
    {
        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 资源路径
        /// </summary>
        public string Href { get; set; }
    }
}
