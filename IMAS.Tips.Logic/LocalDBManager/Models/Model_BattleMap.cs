using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMAS.LocalDBManager.Models
{
    /// <summary>
    /// 战斗地图相关
    /// </summary>
    public class Model_BattleMap
    {
        /// <summary>
        /// 唯一编号
        /// </summary>
        [PrimaryKey]
        [AutoIncrement]
        public int PkId { get; set; }
        /// <summary>
        /// 地图Id
        /// </summary>
        public int MapId { get; set; }
        /// <summary>
        /// 地图名字
        /// </summary>
        public string MapName { get; set; }
        /// <summary>
        /// 限制进入的等级
        /// </summary>
        public int RestrictionLevel { get; set; }

        /// <summary>
        /// 居住的怪物(json格式)
        /// </summary>
        public string LivedMonster { get; set; }
    }
}
