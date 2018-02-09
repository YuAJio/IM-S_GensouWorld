using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMAS.LocalDBManager.Models
{
    public class Model_PlayerControllCharacter : BaseInfo_Character
    {
        /// <summary>
        /// 唯一编号
        /// </summary>
        [PrimaryKey]
        [AutoIncrement]
        public int PkId { get; set; }
        /// <summary>
        /// 是否可以扭出来
        /// </summary>
        public bool IsGacha { get; set; }
        /// <summary>
        /// ガチャ权重
        /// </summary>
        public int GachaWeight { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public long CharacterId { get; set; }
        /// <summary>
        /// 角色立绘地址
        /// </summary>
        public string CharacterTachie { get; set; }
        /// <summary>
        /// 角色装备
        /// </summary>
        public string ChartEquip { get; set; }
        /// <summary>
        /// 角色技能(Json格式)
        /// </summary>
        public string ChartSkills { get; set; }
        /// <summary>
        /// 生命值潜力
        /// </summary>
        public long HP_Potential { get; set; }
        /// <summary>
        /// 魔法值潜力
        /// </summary>
        public long MP_Potential { get; set; }
        /// <summary>
        /// 体力值潜力
        /// </summary>
        public long SP_Potential { get; set; }
        ///// <summary>
        ///// 攻击力潜力
        ///// </summary>
        //public decimal ATT_Potential { get; set; }
        ///// <summary>
        ///// 防御力潜力
        ///// </summary>
        //public decimal DEF_Potential { get; set; }
        /// <summary>
        /// 力量潜力
        /// </summary>
        public decimal STR_Potential { get; set; }
        /// <summary>
        /// 敏捷潜力
        /// </summary>
        public decimal DEX_Potential { get; set; }
        /// <summary>
        /// 智力潜力
        /// </summary>
        public decimal INT_Potential { get; set; }
    }
}
