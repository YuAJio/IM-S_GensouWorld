using System;
using System.Collections.Generic;
using System.Text;

namespace IMAS.LocalDBManager.Models
{
    public class NanaBaseMd
    {
        /// <summary>
        /// 响应码
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 错误码为非200时,是错误信息,错误码是200时,是返回结果
        /// </summary>
        public string message { get; set; }
    }

    /// <summary>
    /// 获取工单信息
    /// </summary>
    public class Md_WorkBillList
    {
        /// <summary>
        /// 工单id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 工单标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 工单状态
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 工单创建时间
        /// </summary>
        public string createtime { get; set; }

    }


}
