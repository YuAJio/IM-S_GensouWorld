using System;
using System.Collections.Generic;
using System.Text;

namespace IMAS.LocalDBManager.Models
{
    /// <summary>
    /// 基础权重类
    /// </summary>
    public class Model_Weight
    {
        /// <summary>
        /// 事件Id
        /// </summary>
        public int EventId { get; set; }
        /// <summary>
        /// 权重
        /// </summary>
        public int PickWeight { get; set; }
    }
}
