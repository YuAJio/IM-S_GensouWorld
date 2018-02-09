using IMAS.Tips.Enums;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMAS.LocalDBManager.Models
{
    public class Model_SearchHistory
    {
        /// <summary>
        /// 唯一编号
        /// </summary>
        [PrimaryKey]
        [AutoIncrement]
        public int pkId { get; set; }

        /// <summary>
        /// 操作向的唯一标识
        /// </summary>
        public InputHistoryType HistoryType { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

    }
}
