using IMAS.Tips.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMAS.LocalDBManager.Models
{
    public class A_BaseInfo
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public bool IsSelect { get; set; }

        public int Weight { get; set; }
    }
    /// <summary>
    /// 可使用角色基础信息
    /// </summary>
    public class BaseInfo_Character
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 最大生命值
        /// </summary>
        public long MaxHealthPoint { get; set; }
        /// <summary>
        /// 最大魔法值
        /// </summary>
        public long MaxManaPoint { get; set; }
        /// <summary>
        /// 最大体力值
        /// </summary>
        public long MaxStaminaPoint { get; set; }
        /// <summary>
        /// 生命值
        /// </summary>
        public long HealthPoint { get; set; }
        /// <summary>
        /// 魔法值
        /// </summary>
        public long ManaPoint { get; set; }
        /// <summary>
        /// 体力值
        /// </summary>
        public long StaminaPoint { get; set; }
        /// <summary>
        /// 攻击力
        /// </summary>
        public long AttackPoint { get; set; }
        /// <summary>
        /// 防御力
        /// </summary>
        public long DefencePoint { get; set; }
        /// <summary>
        /// 力量值
        /// </summary>
        public int STR { get; set; }
        /// <summary>
        /// 敏捷值
        /// </summary>
        public int DEX { get; set; }
        /// <summary>
        /// 智力值
        /// </summary>
        public int INT { get; set; }
        /// <summary>
        /// 经验值
        /// </summary>
        public long Exp { get; set; }
        /// <summary>
        /// 升级所需经验值
        /// </summary>
        public long LevelUpExp { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public int Level { get; set; }
    }
    public class BaseInfo_Skill : A_BaseInfo
    {
        /// <summary>
        /// 技能类型
        /// </summary>
        public SkillsMode SkillMode { get; set; }
        /// <summary>
        /// 技能属性
        /// </summary>
        public SkillsType SkillType { get; set; }
    }

}
