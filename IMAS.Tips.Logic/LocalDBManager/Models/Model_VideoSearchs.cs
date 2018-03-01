using System;
using System.Collections.Generic;
using System.Text;

namespace IMAS.LocalDBManager.Models
{
    public class Model_VideoSearchs
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
}
