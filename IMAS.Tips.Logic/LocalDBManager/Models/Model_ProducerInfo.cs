using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMAS.LocalDBManager.Models
{
    /// <summary>
    /// 制作人实体类
    /// </summary>
    public class Model_ProducerInfo
    {
        /// <summary>
        /// 唯一编号
        /// </summary>
        [PrimaryKey]
        [AutoIncrement]
        public int PkId { get; set; }
        /// <summary>
        /// 制作人名称
        /// </summary>
        public string Prod_Name { get; set; }
        /// <summary>
        /// 制作人等级
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 体力值(用于探险)
        /// </summary>
        public int AP { get; set; }
        /// <summary>
        /// 经验值
        /// </summary>
        public long Exp { get; set; }
        /// <summary>
        /// 距离升级的经验值
        /// </summary>
        public long LevelUpExp { get; set; }
        /// <summary>
        /// マニー
        /// </summary>
        public long Money { get; set; }
        /// <summary>
        /// 制作人生日
        /// </summary>
        public DateTime Prod_Brithday { get; set; }
        ///// <summary>
        ///// 制作人帐号(用于继续游戏)(暂时不必要)
        ///// </summary>
        //public string Prod_Code { get; set; }
        /// <summary>
        /// 制作人证件号码(用于继续游戏)
        /// </summary>
        public long Identity_Number { get; set; }
        /// <summary>
        /// 身上武器道具(Json格式)
        /// </summary>
        public string Items_Weapon { get; set; }
        /// <summary>
        /// 身上装备道具(Json格式)
        /// </summary>
        public string Items_Armor { get; set; }
        /// <summary>
        /// 身上饰品道具(Json格式)
        /// </summary>
        public string Items_Other { get; set; }
        /// <summary>
        /// 身上恢复类道具(Json格式)
        /// </summary>
        public string Items_Healing { get; set; }
        /// <summary>
        /// 目前负责扶持的角色(Json格式)
        /// </summary>
        public string ProducingCharacter { get; set; }

        #region 章节完成度
        /// <summary>
        /// 序章是否完成
        /// </summary>
        public bool IsPrologueChapterFinish { get; set; }
        #endregion

    }
}
