using System;
using System.Collections.Generic;
using System.Text;

namespace IMAS.LocalDBManager.Models
{
    /// <summary>
    /// 聊天内容实体类
    /// </summary>
    public class Model_ChatContent
    {
        /// <summary>
        /// 说的人的Id
        /// </summary>
        public long From_Id { get; set; }
        /// <summary>
        /// 该句触发的事件
        /// </summary>
        public int EventFlag { get; set; }
        /// <summary>
        /// 说的人的名字
        /// </summary>
        public string From_Name { get; set; }
        /// <summary>
        /// 说的内容
        /// </summary>
        public string Content { get; set; }
    }
}
