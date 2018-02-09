using System;
using System.Collections.Generic;
using System.Text;

namespace IMAS.LocalDBManager.Models
{
    public class Model_Skills : BaseInfo_Skill
    {
        /// <summary>
        /// 技能阐释 
        /// </summary>
        public string SkillIllustrate { get; set; }
        /// <summary>
        /// 消耗的生命值
        /// </summary>
        public int CostHp { get; set; }
        /// <summary>
        /// 消耗的魔法值
        /// </summary>
        public int CostMp { get; set; }
        /// <summary>
        /// 消耗的体力值
        /// </summary>
        public int CostSp { get; set; }
        /// <summary>
        /// 伤害
        /// </summary>
        public int Damage { get; set; }
        ///// <summary>
        ///// 物理伤害
        ///// </summary>
        //public int ArmorDamage { get; set; }
        ///// <summary>
        ///// 魔法伤害
        ///// </summary>
        //public int MagicDamage { get; set; }
        ///// <summary>
        ///// 精神伤害
        ///// </summary>
        //public int MindDamage { get; set; }
    }
}
