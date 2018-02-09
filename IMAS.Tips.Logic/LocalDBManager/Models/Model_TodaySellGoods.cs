using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMAS.LocalDBManager.Models
{
    public class Model_TodaySellGoods
    {
        /// <summary>
        /// 唯一编号
        /// </summary>
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// 贩卖日期的标识
        /// </summary>
        public string TodayWeekDate { get; set; }
        /// <summary>
        /// 服装
        /// </summary>
        public string TodaySell_Fuku { get; set; }

        /// <summary>
        /// 饰品
        /// </summary>
        public string TodaySell_Kazari { get; set; }

        /// <summary>
        /// 食物
        /// </summary>
        public string TodaySell_Tabemono { get; set; }
    }
}
